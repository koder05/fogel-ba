using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.Async
{
    /// <summary>
    /// Pool for async loading tasks
    /// </summary>
    internal class AsyncLoadPool
    {
        private Guid _id = Guid.NewGuid();
        private Dictionary<int, CancellationTokenSource> _pool = new Dictionary<int, CancellationTokenSource>();

        internal AsyncLoadPool(Guid id)
        {
            _id = id;
        }

        internal void Clear()
        {
            foreach (int i in _pool.Keys)
            {
                _pool[i].Cancel(false);
            }

            _pool.Clear();
        }

        internal bool Exists(int i)
        {
            return _pool.ContainsKey(i);
        }

        internal void Abort(Predicate<int> match)
        {
            List<int> removedKeys = new List<int>();
            foreach (int i in _pool.Keys)
            {
                if (match(i))
                {
                    _pool[i].Cancel(false);
                    removedKeys.Add(i);
                }
            }

            removedKeys.ForEach(i => this.Remove(i));
        }

        internal bool Put(int i, CancellationTokenSource cts)
        {
            if (this.Exists(i))
                return false;

            //cts.Token.Register(() => Runtime.AbortAsyncTask(new WorkItemId(_id, i)));
            _pool.Add(i, cts);
            return true;
        }

        internal void Remove(int i)
        {
            if (this.Exists(i))
            {
                _pool.Remove(i);
            }
        }
    }
}
