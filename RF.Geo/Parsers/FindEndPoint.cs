using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.Common;
using RF.Geo.BL;

namespace RF.Geo.Parsers
{
    /// <summary>
    /// Класс, представляющий промежуточный результат поиска по ветке шаблона
    /// </summary>
    public class FindEndPoint
    {
        private FindEndPoint _parent;
        /// <summary>
        /// Родительский узел
        /// </summary>
        public FindEndPoint ParentEndPoint 
        {
            get
            {
                return _parent;
            }
            internal set
            {
                _parent = value;
                _parent.AddChild(this);
            }
        }

        private List<FindEndPoint> _childs = new List<FindEndPoint>();
        /// <summary>
        /// Родительский узел
        /// </summary>
        public IEnumerable<FindEndPoint> ChildEndPoints
        {
            get
            {
                return _childs;
            }
        }

        public void AddChild(FindEndPoint child)
        {
            if (!this._childs.Contains(child))
                this._childs.Add(child);
        }

         /// <summary>
        /// Оставшаяся неразобранная часть строки адреса
        /// </summary>
        public string RestTail { get; set; }

        public ResolveRating ResolveRating { get; set; }

        public ObjGeo FoundGeo { get; set; }

        public Location Site { get; set; }

        public AddressTemplate Template { get; set; }
 
        public override bool Equals(object obj)
        {
            if (this.FoundGeo != null && obj is FindEndPoint)
            {
                FindEndPoint point = (FindEndPoint)obj;
                return this.FoundGeo.Equals(point.FoundGeo);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.FoundGeo != null)
                return this.FoundGeo.GetHashCode();

            return base.GetHashCode();
        }
    }
}
