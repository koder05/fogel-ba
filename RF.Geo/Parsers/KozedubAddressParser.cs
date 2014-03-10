using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Security.Cryptography;

using RF.Common;
using RF.Geo.BL;
using RF.Geo.Sql;

namespace RF.Geo.Parsers
{
    public class KozedubAddressParser : IAddressParser
    {
        public static readonly Regex KozedubAddressRx
            = new Regex(@"^(?<Index>\d{6}),\s?(?<Lvl0>[^,]*),\s?(?<Lvl1>[^,]*),\s?(?<Lvl2>[^,]*),\s?(?<Lvl3>[^,]*),\s?(?<Lvl4>[^,]*),\s?(?<Bld>[^,]*),\s?(?<Flat>[^,]*)$"
                , RegexOptions.Compiled);
        public static readonly Regex Lvl14Rx = new Regex(@"^(?<Name>[^,]*)\s(?<Acronym>[^\s,.]*)\.?$", RegexOptions.Compiled);

        private string _postalCode;
        private Location _livingPlace;

        private List<FindEndPoint> currentFoundState = new List<FindEndPoint>();

        /// <summary>
        /// Разбор первого уровня. Уровень собирается в строчку хз как. 
        /// Акроним может быть как в полном, так и в сокращенном виде с точкой или без.
        /// Акроним может быть слева или справа или вообще может отсутсвовать
        /// </summary>
        /// <param name="s">исходные имя и акроним</param>
        /// <returns></returns>
        private bool ParseLvl0(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            string sql = "select * from tblKLADR where GeoType=@lvl and @s like '%' + Name + '%'";
            var founded = DbHelper.Select<ObjGeo>(sql, DbHelper.CreateParameter("@lvl", GeoLevelType.State), DbHelper.CreateParameter("@s", s)).ToList();

            var foundAcronym = new List<string>();
            foreach (var geo in founded)
            {
                if (geo.Name.Equals(s, StringComparison.CurrentCultureIgnoreCase)
                    || string.Format("{0} {1}", geo.AcronymName, geo.Name).Equals(s, StringComparison.CurrentCultureIgnoreCase)
                    || string.Format("{0} {1}", geo.Name, geo.AcronymName).Equals(s, StringComparison.CurrentCultureIgnoreCase)
                    || string.Format("{0}. {1}", geo.AcronymShortName, geo.Name).Equals(s, StringComparison.CurrentCultureIgnoreCase)
                    || string.Format("{0} {1}.", geo.Name, geo.AcronymShortName).Equals(s, StringComparison.CurrentCultureIgnoreCase))
                    if (!foundAcronym.Contains(geo.AcronymName)) foundAcronym.Add(geo.AcronymName);
            }

            currentFoundState.AddRange(AddressParser.CloseLevelFound(null, founded, foundAcronym, new AddressTemplate(0, -1, 0, 0), s));
            return currentFoundState.Any(point => point.FoundGeo != null && point.FoundGeo.Level == GeoLevelType.State);
        }

        /// <summary>
        /// Разбор уровней с региона по улицу.
        /// Формат следующий: имя пробел сокр.акроним точка
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lvl"></param>
        /// <returns></returns>
        private bool ParseLvl14(string s, GeoLevelType lvl)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            if (lvl == GeoLevelType.State)
                return ParseLvl0(s);

            Match m = Lvl14Rx.Match(s);
            string name = m.Groups["Name"].Value;
            string acronym = m.Groups["Acronym"].Value;

            var cmd = DbHelper.CreateCommand("select * from tblKLADR where GeoType=@lvl and Name=@name and AcrShortName=@acr")
                                .AddParameter("@name", name)
                                .AddParameter("@lvl", lvl)
                                .AddParameter("@acr", acronym);
            
            var cutCondition = currentFoundState.Where(point => point.Template.ParentID == -1).Select(point => GeoLevelType.State.CodePart(point.FoundGeo.Code));
            if (!Utils.IsCollectionEmpty(cutCondition))
            {
                cmd.CommandText = cmd.CommandText + " and left(Code,2)=@state_code";
                cmd.ExpandParameterToList<string>("@state_code", cutCondition);
            }

            var founded = cmd.Select<ObjGeo>().ToList();
            var foundAcronym = new List<string>();
            foreach (var geo in founded)
            {
                if (!foundAcronym.Contains(geo.AcronymName)) foundAcronym.Add(geo.AcronymName);
            }

            currentFoundState.AddRange(
                AddressParser.CloseLevelFound(
                    currentFoundState.Where(p => p.ChildEndPoints.Count() == 0)
                        , founded, foundAcronym, new AddressTemplate((int)lvl, (int)lvl - 1, 0, (int)lvl), s));
            return currentFoundState.Any(point => point.FoundGeo != null && point.FoundGeo.Level == lvl);
        }


        /// <summary>
        /// Разбор уровня домов.
        /// </summary>
        /// <returns></returns>
        private void ParseLvl5()
        {
            currentFoundState.AddRange(AddressParser.CloseBldLevelFound(currentFoundState.Where(p => p.ChildEndPoints.Count() == 0), _livingPlace, _postalCode));
        }

        /// <summary>
        /// Разбор номера дома.
        /// </summary>
        /// <param name="s"></param>
        private Location ParseLivingPlace(string input)
        {
            return AddressParser.ParseLocation(input.Replace(".", ""));
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="addr"></param>
        public KozedubAddressParser(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                throw new InvalidOperationException("Cannot parse empty string.");
            SourceAddressString = addr;
        }

        public IEnumerable<Addr> AddressFindList { get; private set; }

        public string SourceAddressString { get; private set; }

        public Addr Parse()
        {
            Match m = KozedubAddressRx.Match(this.SourceAddressString);
            _livingPlace = ParseLivingPlace(AddressParser.QuotatRx.Replace(string.Format("{0} {1}", m.Groups["Bld"].Value, m.Groups["Flat"].Value), ""));
            _postalCode = m.Groups["Index"].Value;
            ParseLvl0(m.Groups["Lvl0"].Value);
            ParseLvl14(m.Groups["Lvl1"].Value, GeoLevelType.Region);
            ParseLvl14(m.Groups["Lvl2"].Value, GeoLevelType.City);
            ParseLvl14(m.Groups["Lvl3"].Value, GeoLevelType.Place);
            ParseLvl14(m.Groups["Lvl4"].Value, GeoLevelType.Street);
            ParseLvl5();

            IEnumerable<FindEndPoint> foundList = currentFoundState.Where(p => p.ChildEndPoints.Count() == 0);
            AddressParser.FinallyCheck(foundList, this._postalCode);

            foreach (var point in foundList)
                point.Site = _livingPlace;

            AddressFindList = foundList.OrderBy(p => p.ResolveRating).Select(p => AddressParser.MapToAddr(p)).ToList();
            return foundList.Any(p => p.ResolveRating == ResolveRating.Clear) ? AddressFindList.ElementAt(0) : null;
        }
    }
}
