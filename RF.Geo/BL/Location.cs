using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RF.Common;
using RF.Geo.BL;
 
namespace RF.Geo.BL
{
    public class Location
    {
        private static readonly Regex BtRx = new Regex(@"^корп$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public string Block { get; set; }

        private string _bt;
        public string BlockType 
        {
            get
            {
                return _bt;
            }
            set
            {
                if (BtRx.IsMatch(value))
                    _bt = value + ".";
                else
                    _bt = value;
            }
        }

        public string Building { get; set; }
        public SiteType SiteType { get; set; }
        public string Site { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(string.Concat(this.Block, this.Building, this.Site));
            }
        }

        public string Presentation 
        {
            get
            {
                StringBuilder sb = new StringBuilder(255);

                if (!string.IsNullOrEmpty(this.Building))
                {
                    sb.Append("д." + this.Building);

                    if (!string.IsNullOrEmpty(this.Block))
                    {
                        sb.Append(this.BlockType + this.Block);
                    }
                }


                if (!string.IsNullOrEmpty(this.Site))
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(Utils.GetEnumDescription<SiteType>(SiteType, true) + Site);
                }

                return sb.ToString();
            }
        }

        public ResolveRating BuildingValidate(ObjGeo geo)
        {
            ResolveRating ret = ResolveRating.BldNomatch;

            if (this.IsEmpty)
                return ret;

            if (false == string.IsNullOrEmpty(this.Block))
            {
                ret = (BuildingShapeValidate(geo, string.Format("{0}/{1}", this.Building, this.Block))
                    || BuildingShapeValidate(geo, string.Format("{0}к{1}", this.Building, this.Block))
                    || BuildingShapeValidate(geo, string.Format("{0}стр{1}", this.Building, this.Block))
                    || BuildingShapeValidate(geo, string.Format("{0}{1}", this.Building, this.Block)))
                    ? ResolveRating.Clear : ret;

                if (ret == ResolveRating.BldNomatch)
                    ret = BuildingShapeValidate(geo, this.Building) ? ResolveRating.BldExcess : ret;
            }
            else
            {
                ret = BuildingShapeValidate(geo, this.Building) ? ResolveRating.Clear : ret;
            }

            return ret;
        }

        private bool BuildingShapeValidate(ObjGeo geo, string shape)
        {
            if (geo.BuildingValidate(shape))
            {
                //this.Building = shape;
                return true;
            }

            return false;
        }
    }
}
