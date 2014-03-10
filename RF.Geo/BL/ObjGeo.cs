using System;
using System.Linq;
using System.Text;
using RegEx = System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml.Serialization;

using RF.Common;
using RF.Geo.Sql;

namespace RF.Geo.BL
{
    [XmlRootAttribute("row")]
    public class ObjGeo
    {
        private const string _buildingNormalizeRegex = @"^(?<home>д(ом)?\.?)?[1-9а-я]*$";
        private const string _buildingRangeRegex = @"^\d+-\d+$";
        private const string _buildingEvenRangeRegex = @"^Ч(\((?<Range>\d+-\d+)\))?$";
        private const string _buildingOddRangeRegex = @"^Н(\((?<Range>\d+-\d+)\))?$";

        [XmlAttribute("BranchInitalCode")]
        public string BranchInitalCode { get; set; }
        [XmlAttribute("Code")]
        public string Code { get; set; }
        [XmlAttribute("ParentCode")]
        public string ParentCode { get; set; }
        [XmlAttribute("OriginalCode")]
        public string OriginalCode { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Zip")]
        public string Index { get; set; }
        [XmlAttribute("GeoType")]
        public byte LevelValue { get; set; }
        [XmlAttribute("AcrName")]
        public string AcronymName { get; set; }
        [XmlAttribute("AcrShortName")]
        public string AcronymShortName { get; set; }
        [XmlAttribute("Actual")]
        public byte Actual { get; set; }

        public GeoLevelType Level
        {
            get
            {
                return (GeoLevelType)LevelValue;
            }
        }

        public string BuildingTemplateString
        {
            get
            {
                if (this.Level == GeoLevelType.Building)
                    return this.Name;

                return string.Empty;
            }
        }

        public string FullName
        {
            get
            {
                if (false == this.AcronymShortName.Equals("г", StringComparison.OrdinalIgnoreCase))
                    switch (this.Level)
                    {
                        case GeoLevelType.State:
                        case GeoLevelType.Region:
                        case GeoLevelType.Place:
                        case GeoLevelType.Street:
                            return string.Format("{0} {1}.", this.Name, this.AcronymShortName);
                    }

                return string.Format("{0}. {1}", this.AcronymShortName, this.Name);
            }
        }

        private IEnumerable<ObjGeo> _fullAddressSubjectPool = null;

        [XmlIgnore]
        public IEnumerable<ObjGeo> FullAddressSubjectPool
        {
            get
            {
                if (_fullAddressSubjectPool == null)
                {
                    //_fullAddressSubjectPool = IoC.Resolve<IGeoRepository>().GetFullAddressById(this.ID).OrderBy(o => o.Level);
                }

                return _fullAddressSubjectPool;
            }
            set
            {
                _fullAddressSubjectPool = value;
            }
        }

        public string FullAddressStringWithoutIndex
        {
            get
            {
                StringBuilder address = new StringBuilder();

                foreach (ObjGeo o in FullAddressSubjectPool)
                {
                    if (o.Level != GeoLevelType.Building)
                        address.AppendFormat("{0}, ", o.FullName);
                }

                while (address.Length > 0 && (address[address.Length - 1] == ',' || address[address.Length - 1] == ' '))
                    address.Remove(address.Length - 1, 1);

                return address.ToString();
            }
        }

        public string FullAddressString
        {
            get
            {
                StringBuilder index = new StringBuilder();
                StringBuilder address = new StringBuilder();

                foreach (ObjGeo o in FullAddressSubjectPool)
                {
                    if (!string.IsNullOrEmpty(o.Index))
                    {
                        index.Remove(0, index.Length);
                        index.Append(o.Index);
                    }

                    if (o.Level != GeoLevelType.Building)
                        address.AppendFormat("{0}, ", o.FullName);
                }

                if (index.Length > 0)
                    address.Insert(0, string.Format("{0}, ", index.ToString()));

                while (address.Length > 0 && (address[address.Length - 1] == ',' || address[address.Length - 1] == ' '))
                    address.Remove(address.Length - 1, 1);

                return address.ToString();
            }
        }

        public short RegionCode
        {
            get
            {
                short ret = 0;
                short.TryParse(this.Code.Substring(0, 2), out ret);
                return ret;
            }
        }

        public string GetNormalizeBuildingString(string buildingExpr)
        {
            string res = string.Empty;

            if (false == string.IsNullOrEmpty(buildingExpr))
            {
                res = buildingExpr.Replace(" ", "");
                RegEx.Regex r = new RegEx.Regex(_buildingNormalizeRegex, RegEx.RegexOptions.IgnoreCase);
                RegEx.Match m = r.Match(res);
                if (m.Success && !string.IsNullOrEmpty(m.Groups["home"].Value))
                    res = res.Replace(m.Groups["home"].Value, "");
            }

            return res;
        }

        private bool FindNumberInRange(int number, string range, bool isEvenRange, bool isOddRange)
        {
            if (string.IsNullOrEmpty(range))
                range = "0-999";
            string[] rangeLims = range.Split('-');
            int rb = Convert.ToInt32(rangeLims[0]);
            int re = Convert.ToInt32(rangeLims[1]);
            if (false == isEvenRange && false == isOddRange && number >= rb && number <= re)
            {
                return true;
            }

            int remainder = 0;
            Math.DivRem(number, 2, out remainder);
            if (((isEvenRange && remainder == 0) || (isOddRange && remainder > 0)) && number >= rb && number <= re)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Можно ли давать выбирать subject при наличии дочерних объектов
        /// <remarks>Можно, если: 
        /// 1. все дочерние объекты с тем же индексом; 
        /// 2. для сельск. местности канает если дочерние отличаются индексом но этот индекс у всех них одинаковый </remarks>
        /// </summary>
        public bool HasChildSubjectWithOtherIndex
        {
            get
            {
                bool ret = false;
                //var childList = IoC.Resolve<IGeoRepository>().GetChildList(this.ID, null, null, 0, 0, null);
                //ret = childList.Any(g => g.Index != this.Index);

                //if (ret && false == string.IsNullOrEmpty(this.Index) && this.Level == GeoLevelType.Place && 1 == childList.GroupBy(g => g.Index).Count())
                //    ret = false;

                return ret;
            }
        }

        /// <summary>
        /// Проверка введенного номера здания на соответствие выражению в КЛАДР
        /// </summary>
        /// <param name="buildingExpr"></param>
        /// <returns></returns>
        public bool BuildingValidate(string buildingExpr)
        {
            string expr = GetNormalizeBuildingString(buildingExpr);

            RegEx.Regex isWholeRegex = new RegEx.Regex(@"^(?i)[\d\w/-]*$");
            RegEx.Regex isDigitRegex = new RegEx.Regex(@"^(?<Num>\d+)");
            RegEx.Regex isRangeRegex = new RegEx.Regex(_buildingRangeRegex);
            RegEx.Regex isEvenRangeRegex = new RegEx.Regex(_buildingEvenRangeRegex);
            RegEx.Regex isOddRangeRegex = new RegEx.Regex(_buildingOddRangeRegex);

            if (string.IsNullOrEmpty(this.BuildingTemplateString))
            {
                return isWholeRegex.IsMatch(expr);
            }

            string[] templateList = this.BuildingTemplateString.Split(',');

            //простое сравнение
            foreach (string s in templateList)
            {
                if (expr.Equals(s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            //если простое сравнение не удалось, отсеиваем буквы, корпуса и строения (в кладр темплейты номеров домов часто не учитывают букв и корпусов); 
            //повторяем прямое сравнение + ищем номер в диапазонах

            //удостоверимся, что выражение на входе есть простое число
            RegEx.Match m = isDigitRegex.Match(expr);
            if (m.Groups["Num"] != null && m.Groups["Num"].Success)
            {
                int number = Convert.ToInt32(m.Groups["Num"].Value);
                foreach (string s in templateList)
                {
                    if (m.Groups["Num"].Value.Equals(s, StringComparison.OrdinalIgnoreCase))
                        return true;

                    if (
                        (isRangeRegex.IsMatch(s) && FindNumberInRange(number, s, false, false))
                        || (isEvenRangeRegex.IsMatch(s) && FindNumberInRange(number, isEvenRangeRegex.Match(s).Groups["Range"].Value, true, false))
                        || (isOddRangeRegex.IsMatch(s) && FindNumberInRange(number, isOddRangeRegex.Match(s).Groups["Range"].Value, false, true))
                    )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            ObjGeo geo = obj as ObjGeo;
            if (geo != null)
            {
                return ((ObjGeo)obj).OriginalCode.Equals(this.OriginalCode);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.OriginalCode.GetHashCode();
        }

        public static ObjGeo GetByCode(string code)
        {
            string sql = "select * from tblKLADR where c.Code = @code";
            var o = DbHelper.Select<ObjGeo>(sql, DbHelper.CreateParameter("@code", code)).FirstOrDefault(g => g.Actual==0);
            return o;
        }
    }
}
