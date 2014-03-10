using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

using Common.Async;
using RF.LinqExt;

namespace RF.WinApp.JIT
{
    internal class Page
    {
        internal IEnumerable<DataObj> Table;
        internal int Number;

        internal Page(int number)
        {
            this.Number = number;
        }
    }

    internal struct NI
    {
        internal int N;
        internal int I;

        internal NI(int n, int i)
        {
            N = n;
            I = i;
        }
    }

    public class PageManager
    {
        private Guid _id = Guid.NewGuid();
        public Guid StaticStateVersion
        {
            get
            {
                return _id;
            }
        }

        private int _pageSize;
        private int _cacheDeep;
        private int _pageMaxNumber;
        private Page[] _cache;
        private NI[] _currentTwoPagesPool = new NI[2];
        private IDataView _dataSupply;
        private AsyncLoadPool _asyncPool;

        private FilterParameterCollection _filters = null;
        public FilterParameterCollection Filters
        {
            get
            {
                return _filters;
            }
            set
            {
                _filters = value;
                _rowCount = -1;
                //ResetPages();
            }
        }

        private SortParameterCollection _sort = new SortParameterCollection();
        public void AddSortOrder(string columnName, ListSortDirection dir)
        {
            _sort.Add(this._dataSupply.ModelType, columnName, dir);
        }

        public void RemoveSortOrder(string columnName)
        {
            _sort.Remove(this._dataSupply.ModelType, columnName);
        }

        private int _rowCount = -1;
        public int RowCount
        {
            get
            {
                if (_rowCount < 0)
                {
                    _rowCount = _dataSupply.GetListCount(_filters);
                }

                return _rowCount;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
        }

        public PageManager(int pageSize, int cacheDeep, int cacheWidth, IDataView dataSupply)
        {
            if (pageSize <= 0)
                throw new Exception("pageSize");
            if (cacheDeep < 0)
                throw new Exception("cacheDeep");

            _pageSize = pageSize;
            _cacheDeep = cacheDeep;
            _cache = new Page[Math.Max(cacheDeep * 2 + 2, cacheWidth)];
            _dataSupply = dataSupply;
            _asyncPool = new AsyncLoadPool(_id);

            //ResetPages();
        }

        public void Reset()
        {
            this._rowCount = -1;
            ResetPages();
        }

        public void ResetPages()
        {
            //abort all async tasks
            _asyncPool.Clear();
            _pageMaxNumber = this.RowCount / _pageSize;

            lock (_cache)
            {
                for (int i = 0; i < _cache.Length; i++)
                {
                    _cache[i] = new Page(_pageMaxNumber * 3 + 1);
                }

                _currentTwoPagesPool[0] = new NI(_pageMaxNumber * 3 + 1, 0);
                _currentTwoPagesPool[1] = new NI(_pageMaxNumber * 3 + 2, 0);
            }

            _id = Guid.NewGuid();
        }

        public DataObj RetrieveElement(int rowIndex)
        {
            DataObj ret = null;
            int needPageNumber = rowIndex / _pageSize;
            int pageIndexInCache = GetPageIndexInCache(needPageNumber);
            
            if (pageIndexInCache >= 0 && pageIndexInCache < _cache.Count())
            {
                ret = GetValue(_cache[pageIndexInCache], rowIndex % _pageSize);
            }

            return ret;
        }

        public int GetElementIndex(object o)
        {
            if (o == null)
                return 0;

            return _dataSupply.GetIndexOf(o, _filters, _sort);
        }

        private int GetPageIndexInCache(int needPageNumber)
        {
            int ret = -1;
            if (_currentTwoPagesPool[0].N == needPageNumber)
            {
                ret = _currentTwoPagesPool[0].I;
            }
            else if (_currentTwoPagesPool[1].N == needPageNumber)
            {
                ret = _currentTwoPagesPool[1].I;
            }
            else
            {
                RenewCurrentPool(needPageNumber);
                if (_currentTwoPagesPool[0].N == needPageNumber)
                {
                    ret = _currentTwoPagesPool[0].I;
                }
                else if (_currentTwoPagesPool[1].N == needPageNumber)
                {
                    ret = _currentTwoPagesPool[1].I;
                }
            }
            return ret;
        }

        private void RenewCurrentPool(int needPageNumber)
        {
            //aborting old tasks
            _asyncPool.Abort(i => Math.Abs(i - needPageNumber) > _cacheDeep + 1);
            //wait need page async load end
            while (_asyncPool.Exists(needPageNumber))
            {
                Thread.Sleep(100);
            }

            lock (_cache)
            {
                int index = FindIndex(needPageNumber);
                if (index < 0)
                {
                    //sync load
                    LoadPage(needPageNumber);
                    index = FindIndex(needPageNumber);
                    if (index < 0)
                        throw new Exception("sync load");
                }

                int indexOnRemove = 0;
                if (Math.Abs(_currentTwoPagesPool[0].N - needPageNumber) < Math.Abs(_currentTwoPagesPool[1].N - needPageNumber))
                    indexOnRemove = 1;
                _currentTwoPagesPool[indexOnRemove] = new NI(needPageNumber, index);
            }

            for (int i = 1; i <= _cacheDeep; i++)
            {
                //async load needPageNumber + i
                AsyncLoadPage(needPageNumber + i);
                //async load needPageNumber - i
                AsyncLoadPage(needPageNumber - i);
            }
        }

        private void LoadPage(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > _pageMaxNumber)
                return;
            IEnumerable<DataObj> data = _dataSupply.GetList(_filters, pageNumber, _pageSize, this._sort).Select(o => new DataObj() { Model = o, StaticStateVersion = this._id });
            PutPageInCache(pageNumber, data);
        }

        private void AsyncLoadPage(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > _pageMaxNumber)
                return;

            if (!_cache.Any(p => p.Number == pageNumber) && !_asyncPool.Exists(pageNumber))
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                _asyncPool.Put(pageNumber, cts);
                Task<IEnumerable<DataObj>> task = Task.Factory.StartNew<IEnumerable<DataObj>>(
                    () => _dataSupply.GetList(_filters, pageNumber, _pageSize, this._sort).Select(o => new DataObj() { Model = o, StaticStateVersion = this._id })
                    , cts.Token);
                task.ContinueWith((t) => AsyncResponse(t, pageNumber), CancellationToken.None);
            }
        }

        private void AsyncResponse(Task<IEnumerable<DataObj>> task, int pageNumber)
        {
            _asyncPool.Remove(pageNumber);
            if (task.Status == TaskStatus.RanToCompletion)
                PutPageInCache(pageNumber, task.Result);
        }

        private int FindIndex(int number)
        {
            int ret = -1;
            for (int i = 0; i < _cache.Length; i++)
            {
                if (_cache[i].Number == number)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private DataObj GetValue(Page page, int rowIndex)
        {
            DataObj ret = null;
            if (page != null && page.Table != null && page.Table.Count() > rowIndex)
                ret = page.Table.ElementAt(rowIndex);
            return ret;
        }

        private void PutPageInCache(int pageNumber, IEnumerable<DataObj> data)
        {
            if (data.Count() > 0)
            {
                //rewrite oldest page in cache
                lock (_cache)
                {
                    int indexOnRemove = 0;
                    int delta = 0;
                    for (int i = 0; i < _cache.Length; i++)
                    {
                        if (_currentTwoPagesPool[0].I != i && _currentTwoPagesPool[1].I != i)
                        {
                            int d = Math.Abs(_cache[i].Number - pageNumber);
                            if (delta < d)
                            {
                                indexOnRemove = i;
                                delta = d;
                            }
                        }
                    }
                    _cache[indexOnRemove].Table = data;
                    _cache[indexOnRemove].Number = pageNumber;
                }
            }
        }
    }
}
