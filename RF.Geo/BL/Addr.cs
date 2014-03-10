using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

using RF.Common;
using RF.Geo.Sql;
using RF.BL.Model;
using RF.Geo.Parsers;
using RF.LinqExt;

namespace RF.Geo.BL
{
    [XmlRootAttribute("row")]
    public class Addr : BaseModel
    {
        [XmlAttribute("Id")]
        public int AddrId { get; set; }

        [XmlAttribute("OriginalString")]
        public string OriginalString { get; set; }

        [XmlAttribute("OriginalCode")]
        public string OriginalCode { get; set; }

        [XmlAttribute("TrueCode")]
        public string TrueCode { get; set; }

        [XmlAttribute("HouseFlat")]
        public string HouseFlat { get; set; }

        private IEnumerable<ObjGeo> _fullAddressSubjectPool = null;
        [XmlIgnore]
        public IEnumerable<ObjGeo> FullAddressSubjectPool
        {
            get
            {
                if (_fullAddressSubjectPool == null)
                {
                    _fullAddressSubjectPool = DbHelper.Select<ObjGeo>(
@"with cte(Code,ParentCode) as 
(
select Code, ParentCode from tblKLADR where Code=@code and Actual=0 union all select k.Code, k.ParentCode from tblKLADR k, cte where cte.ParentCode=k.Code and Actual=0
)
select k.* from tblKLADR k, cte where cte.Code = k.Code and Actual=0 order by GeoType"
                    , DbHelper.CreateParameter("@code", this.TrueCode)).ToList();
                }

                return _fullAddressSubjectPool;
            }
            set
            {
                _fullAddressSubjectPool = value;
            }
        }

        [XmlIgnore]
        public string Code { get { return FullAddressSubjectPool.OrderByDescending(geo => geo.Level).Select(geo => geo.Code).FirstOrDefault(); } }
        [XmlIgnore]
        public string Zip { get { return FullAddressSubjectPool.Where(geo => !string.IsNullOrEmpty(geo.Index)).OrderByDescending(geo => geo.Level).Select(geo => geo.Index).FirstOrDefault(); } }
        [XmlIgnore]
        public ObjGeo State { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.State); } }
        [XmlIgnore]
        public ObjGeo Region { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.Region); } }
        [XmlIgnore]
        public ObjGeo City { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.City); } }
        [XmlIgnore]
        public ObjGeo Place { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.Place); } }
        [XmlIgnore]
        public ObjGeo Street { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.Street); } }
        [XmlIgnore]
        public ObjGeo Building { get { return FullAddressSubjectPool.FirstOrDefault(geo => geo.Level == GeoLevelType.Building); } }


        [XmlIgnore]
        public IEnumerable<Addr> ParseResult { get; private set; }

        public Addr Parse()
        {
            var parser = new AddressParserFactory().GetParser(this.OriginalString);
            Addr found = parser.Parse();

            ParseResult = parser.AddressFindList;
            OnPropertyChanged("ParseResult");

            if (found != null)
            {
                TrueCode = found.TrueCode;
                OnPropertyChanged("TrueCode");
            }

            if (!Utils.IsCollectionEmpty(ParseResult))
            {
                HouseFlat = ParseResult.First().HouseFlat;
                OnPropertyChanged("HouseFlat");
            }

            return found;
        }

        public static IEnumerable<Addr> GetList(FilterParameterCollection filters, int pageIndex, int pageSize)
        {
            return DbHelper.CreateCommand("select * from tblAddress").AddFiltering(filters).AddPaging(pageIndex, pageSize, "KodPS, AddrType").Select<Addr>().ToList();
        }

        public static int GetListCount(FilterParameterCollection filters)
        {
            return DbHelper.CreateCommand("select * from tblAddress").AddFiltering(filters).SelectCount();
        }

        public static void Save(Addr a)
        {
            DbHelper.CreateCommand("update tblAddress set TrueCode=@tcode, HouseFlat=@hf where Id=@id")
                .AddParameter("@tcode", a.TrueCode)
                .AddParameter("@hf", a.HouseFlat)
                .AddParameter("@id", a.AddrId)
                .Execute();
        }
    }
}
