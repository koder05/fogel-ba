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
    public class AddressParser : IAddressParser
    {
        #region Regular Expr

        internal static readonly Regex QuotatRx = new Regex("[\"']", RegexOptions.Compiled);
        internal static readonly Regex CommaRx = new Regex("[.,\"']", RegexOptions.Compiled);
        internal static readonly Regex SpaceRx = new Regex(@"\s{2,}", RegexOptions.Compiled);
        private static readonly Regex PostalCodeRx = new Regex(@"(?<idx>\d{6})", RegexOptions.Compiled);

        internal static readonly Regex PostBoxRx = new Regex(@"а/я\s?(?<postbox>\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal static readonly Regex MilitaryBaseRx = new Regex(@"в[ \\/-]?ч\s?(?<milbase>\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal static readonly Regex BldRx = new Regex(@"^(?(\D)(.*(ж/дом|д(ом)?|№)\s?)|\A)(?<bld>\d+[а-ж]?([\\/][\dа-ж]+)*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal static readonly Regex BlockRx = new Regex(@"^(?([^0-9-а-ж])(.*(?<bt>крп|кор(п(ус)?)?|(?<![а-я].*)к(?!в)|стр(оение)?|блок|(?<![а-я].*)общ(ежитие)?)\s?)(?<block>[а-щ\d]+([ /-][а-ж\d]+)*)|(?(-?\d+[а-ж]?(\s(?>!блок)|\Z))(#)|(-?(?<block>[а-ж\d]+(/[а-ж\d]+)*))))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal static readonly Regex FlatRx = new Regex(@"^(?([^0-9-])(.*(?<flattype>(кв(артира)?|(?<!кв.*)к(ом(н(ата)?)?)?|лит(ера)?))\s?)|\A)-?(?<flat>[\dа-я]+(-[\dа-я])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        /// <summary>
        /// Нормализованная входная строка (без мусорных символов, между словами строго один пробел)
        /// </summary>
        private string NormalizedAddressString { get; set; }

        /// <summary>
        /// Промежуточный результат поиска имени объекта по одной ноде шаблона
        /// Содержит точное полное имя акронима + пробел + точное полное имя объекта
        /// </summary>
        private List<string> IntermediateNameFindResult = new List<string>();

        /// <summary>
        /// Промежуточный результат поиска имени акронима по одной ноде шаблона
        /// Содержит точное полное имя акронима
        /// </summary>
        private List<string> IntermediateAcronymFindResult = new List<string>();

        /// <summary>
        /// Результирующий набор последних найденных имен. Каждому уровню соответсвует список:
        /// точное полное имя акронима + пробел + точное полное имя объекта
        /// </summary>
        private List<FindEndPoint> FindResult = new List<FindEndPoint>();

        private IEnumerable<AddressTemplate> _tmpList;
        /// <summary>
        /// Набор шаблонов для поиска
        /// </summary>
        private IEnumerable<AddressTemplate> Templates
        {
            get
            {
                if (_tmpList == null)
                    _tmpList = AddressTemplate.GetList();
                return _tmpList;
            }
        }

        /// <summary>
        /// Входная строка
        /// </summary>
        public string SourceAddressString { get; private set; }

        /// <summary>
        /// Найденный почтовый индекс
        /// </summary>
        private string PostalCode { get; set; }

        private Location Site { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="addr"></param>
        public AddressParser(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                throw new InvalidOperationException("Cannot parse empty string.");
            SourceAddressString = addr;
        }

        private string GetSpacedStringFromArray(string[] arr, int start, int len)
        {
            string ret = string.Empty;
            if (len > 0 && start >= 0 && start + len <= arr.Length)
            {
                if (len == 1)
                    ret = arr[start];
                else
                {
                    StringBuilder b = new StringBuilder(arr.Sum(s => s.Length) + arr.Length);
                    b.AppendFormat("{0}", arr[start]);
                    for (int i = 1; i < len; i++)
                        b.AppendFormat(" {0}", arr[start + i]);

                    ret = b.ToString();
                }
            }
            return ret;
        }

        private string GetStringFromArray(string[] arr, int start, int len)
        {
            string ret = string.Empty;
            if (len > 0 && start >= 0 && start + len <= arr.Length)
            {
                if (len == 1)
                    ret = arr[start];
                else if (len == 2)
                    ret = string.Concat(arr[start], " ", arr[start + 1]);
                else
                {
                    StringBuilder b = new StringBuilder(arr.Sum(s => s.Length));
                    for (int i = 0; i < len; i++)
                        b.Append(arr[start + i] + " ");

                    ret = b.ToString().TrimEnd();
                }
            }
            return ret;
        }

        /// <summary>
        /// Метод нормализации
        /// </summary>
        private void NormalizeAddressString()
        {
            NormalizedAddressString = CommaRx.Replace(SourceAddressString.ToLower(), " ");
            NormalizedAddressString = QuotatRx.Replace(NormalizedAddressString, "");
            NormalizedAddressString = SpaceRx.Replace(NormalizedAddressString, " ");
        }

        /// <summary>
        /// Метод поиска почтового индекса
        /// </summary>
        private void ExtractPostalCode()
        {
            Match m = PostalCodeRx.Match(NormalizedAddressString);
            if (m.Groups["idx"].Success)
                PostalCode = m.Groups["idx"].Value;
        }

        /// <summary>
        /// Рекурсивно ищет объект указанного уровня во входящей строке адреса
        /// </summary>
        /// <param name="level">Уровень объекта</param>
        /// <param name="findPos">С какого слова начинать искать</param>
        /// <param name="findLen">Строку из скольки слов ищем</param>
        /// <param name="findArr">Строка адреса, разбитая на слова по пробелу</param>
        /// <param name="foundLen">Сколько слов в итоге соответствует имени объекта, 0 - ничего не нашли</param>
        /// <returns>Список объектов удовлетворяющих поиску</returns>
        private IEnumerable<ObjGeo> FindName(int level, int findPos, int findLen, string[] findArr, out int foundLen)
        {
            string findString = GetStringFromArray(findArr, findPos, findLen);
            foundLen = 0;

            // если искать больше негде - не нашли
            if (string.IsNullOrEmpty(findString))
                return null;

            // ищем по like 'findString%' c увеличением строки поиска на единицу (шоб найти типа "Маршала Конева", при изначальной findString = "Маршала")
            string findLikeString = GetStringFromArray(findArr, findPos, findLen + 1);
            if (string.IsNullOrEmpty(findLikeString) == false)
            {
                var i = DbHelper.SelectCount("select * from tblKLADR where Name like @s and GeoType=@lvl"
                            , DbHelper.CreateParameter("@lvl", level)
                            , DbHelper.CreateParameter("@s", findLikeString + "%"));
                if (i > 0)
                {
                    // если по like 'findString%' нашли - ищем дальше, увеличивая искомое словочетание на одно слово 
                    return FindName(level, findPos, findLen + 1, findArr, out foundLen);
                }
            }

            //мы здесь - значит строка поиска уже исчерпана или like по строке, увеличенной на 1 слово ничего не нашёл
            // ищем по прямому стравнению
            var listEqCmd = DbHelper.CreateCommand("select * from tblKLADR where Name=@s and GeoType=@lvl").AddParameter("@lvl", level).AddParameter("@s", findString);
            var cutList = currentFoundState.Values.SelectMany(p => p).Where(point => point.Template != null && point.Template.ParentID == -1).Select(point => GeoLevelType.State.CodePart(point.FoundGeo.Code));
            if (!Utils.IsCollectionEmpty(cutList))
            {
                listEqCmd.CommandText = listEqCmd.CommandText + "  and left(Code,2)=@state_code";
                listEqCmd.ExpandParameterToList("@state_code", cutList);
            }
            IEnumerable<ObjGeo> listEq = listEqCmd.Select<ObjGeo>().ToList();

            if (listEq != null && listEq.Count() > 0)
            {
                foundLen = findLen;
                return listEq;
            }

            return null;
        }

        /// <summary>
        /// Рекурсивно ищет акроним указанного уровня во входящей строке адреса
        /// </summary>
        /// <param name="level">Уровень объекта акронима</param>
        /// <param name="findPos">С какого слова начинать искать</param>
        /// <param name="findLen">Строку из скольки слов ищем</param>
        /// <param name="findArr">Строка адреса, разбитая на слова по пробелу</param>
        /// <param name="foundLen">возврат: Сколько слов в итоге соответствует акрониму, 0 - ничего не нашли</param>
        /// <returns>список полных имен акронимов по условиям поиска</returns>
        private IEnumerable<string> FindAcronym(int level, int findPos, int findLen, string[] findArr, out int foundLen)
        {
            string findString = GetStringFromArray(findArr, findPos, findLen);
            foundLen = 0;

            // если искать больше негде - не нашли
            if (string.IsNullOrEmpty(findString))
            {
                return null;
            }

            // ищем по прямому стравнениею
            IEnumerable<string> list = DbHelper.SelectScalarList<string>("select SocrName from tbl_KLADR_SocrBase where [Level]=@lvl and (replace(SocrName,' ','')=@name or replace(SCName,' ','')=@name)"
                    , DbHelper.CreateParameter("@lvl", level + 1), DbHelper.CreateParameter("@name", findString));

            if (list == null || list.Count() == 0)
            {
                // если не нашли, ищем по like 'findString%' 
                int i = DbHelper.SelectCount("select SocrName from tbl_KLADR_SocrBase where [Level]=@lvl and (replace(SocrName,' ','') like @name or replace(SCName,' ','') like @name)"
                    , DbHelper.CreateParameter("@lvl", level + 1), DbHelper.CreateParameter("@name", findString + "%"));
                if (i > 0)
                {
                    // если по like 'findString%' нашли - ищем дальше, увеличивая искомое словочетание на одно слово 
                    return FindAcronym(level, findPos, findLen + 1, findArr, out foundLen);
                }

                // ничего не нашли
                return null;
            }
            else
            {
                // прямое сравнение отработало
                foundLen = findLen;
                return list;
            }
        }

        /// <summary>
        /// Ищем объект указанного уровня вместе с акронимами во входящей строке адреса
        /// </summary>
        private bool FindLevel(ref string restTail, AddressTemplate tmpl)
        {
            if (string.IsNullOrEmpty(restTail))
                return false;

            string[] addrParts = restTail.Split(' ');

            //ищем акроним сначала
            int foundLen = 0;
            IEnumerable<ObjGeo> foundList = null;
            var acronyms = FindAcronym(tmpl.Level, 0, 1, addrParts, out foundLen);
            if (foundLen == 0)
            {
                //акронима сначала не нашли - ищем имя объекта
                foundList = FindName(tmpl.Level, 0, 1, addrParts, out foundLen);
                if (foundLen > 0)
                {
                    //если имя найдено - ищем акроним после имени
                    int foundLen2 = 0;
                    acronyms = FindAcronym(tmpl.Level, 0 + foundLen, 1, addrParts, out foundLen2);
                    foundLen += foundLen2;
                }
            }
            else
            {
                //нашли акроним, далее ищем имя объекта с позиции после акронима
                int foundLen2 = 0;
                foundList = FindName(tmpl.Level, 0 + foundLen, 1, addrParts, out foundLen2);

                if (foundLen2 == 0)
                {
                    //если имени не нашли, то толку от найденного акронима нет. 
                    //считаем, что вообще ничего не нашли
                }
                else
                {
                    foundLen += foundLen2;
                }
            }

            restTail = GetStringFromArray(addrParts, foundLen, addrParts.Length - foundLen);
            if (currentFoundState.ContainsKey(tmpl.ID) == false)
                currentFoundState.Add(tmpl.ID, new List<FindEndPoint>());

            if (currentFoundState.ContainsKey(tmpl.ParentID))
                currentFoundState[tmpl.ID].AddRange(CloseLevelFound(currentFoundState[tmpl.ParentID], foundList, acronyms, tmpl, restTail));
            else
                currentFoundState[tmpl.ID].AddRange(CloseLevelFound(null, foundList, acronyms, tmpl, restTail));

            return currentFoundState[tmpl.ID].Any(point => point.FoundGeo != null && (int)point.FoundGeo.Level == tmpl.Level);
        }

        /// <summary>
        /// Разрбор строки на предмет номера дома-квартиры
        /// </summary>
        private Location ParseLivingPlace(string restTail, AddressTemplate tmpl)
        {
            Location found = ParseLocation(restTail);
            if (currentFoundState.ContainsKey(tmpl.ID) == false)
                currentFoundState.Add(tmpl.ID, new List<FindEndPoint>());

            if (currentFoundState.ContainsKey(tmpl.ParentID))
                currentFoundState[tmpl.ID].AddRange(CloseBldLevelFound(currentFoundState[tmpl.ParentID], found, this.PostalCode));

            return found;
        }

        /// <summary>
        /// Результаты поиска, ключ - номер ветки темплейта
        /// </summary>
        private Dictionary<int, List<FindEndPoint>> currentFoundState = new Dictionary<int, List<FindEndPoint>>();

        /// <summary>
        /// Перебираем ноды одной ветки шаблона, при успешном разрешении ноды, рекурсивно спускаемся вниз по дереву
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="findStart"></param>
        private void ParseBranch(string restTail, int parentNodeId)
        {
            foreach (var branchNode in this.Templates.Where(t => t.ParentID == parentNodeId))
            {
                // 0-уровень субъекта РФ
                // 1-уровень региона
                // 2-уровень города
                // 3-уровень деревни, района
                // 4-уровень улицы
                // 5-уровень домов, здесь алгоритм поиска меняется координально
                string s = restTail.ToString();
                if (branchNode.Level < (int)GeoLevelType.Building)
                {
                    if (FindLevel(ref s, branchNode))
                        ParseBranch(s, branchNode.ID);
                }
                else
                {
                    var site = ParseLivingPlace(s, branchNode);
                    foreach (var point in currentFoundState.Values.SelectMany(p => p).Where(p => p.ChildEndPoints.Count() == 0 && p.Site == null))
                        point.Site = site;
                }
            }
        }

        public IEnumerable<Addr> AddressFindList { get; private set; }

        public Addr Parse()
        {
            NormalizeAddressString();
            ExtractPostalCode();

            if (false == string.IsNullOrEmpty(this.PostalCode))
            {
                this.NormalizedAddressString = this.NormalizedAddressString.Replace(this.PostalCode, "");
            }

            this.NormalizedAddressString = this.NormalizedAddressString.Trim();

            ParseBranch(this.NormalizedAddressString, -1);

            IEnumerable<FindEndPoint> foundList = currentFoundState.Values.SelectMany(p => p).Where(p => p.ChildEndPoints.Count() == 0);

            FinallyCheck(foundList, this.PostalCode);

            AddressFindList = foundList.OrderBy(p => p.ResolveRating).Select(p => MapToAddr(p)).ToList();
            return foundList.Any(p => p.ResolveRating == ResolveRating.Clear) ? AddressFindList.ElementAt(0) : null;
        }

        internal static Addr MapToAddr(FindEndPoint point)
        {
            if (point == null || point.FoundGeo == null)
                return null;

            Addr ret = new Addr();
            ret.TrueCode = point.FoundGeo.Code;
            ret.HouseFlat = (point.Site ?? new Location()).Presentation;
            return ret;
        }

        internal static ResolveRating EnsureParentChildConsistency(ObjGeo parent, ObjGeo child)
        {
            if (parent.Level >= child.Level)
                throw new ArgumentOutOfRangeException("parent.Level >= child.Level");

            ResolveRating ret = ResolveRating.Clear;
            string childCode = child.Level.CodeTrim(child.Code);

            if (childCode.StartsWith((child.Level - 1).CodeTrim(parent.Code)))
                return ret;

            if (childCode.StartsWith(parent.Level.CodeTrim(parent.Code)))
            {
                ret |= ResolveRating.Shortage;
                return ret;
            }

            if (parent.Level.CodePart(child.Code).All(ch => ch == '0'))
                ret |= ResolveRating.Excess;
            else
                ret |= ResolveRating.NoMatch;

            return ret;
        }

        internal static IEnumerable<FindEndPoint> CloseLevelFound(IEnumerable<FindEndPoint> parents, IEnumerable<ObjGeo> foundList, IEnumerable<string> foundAcronym, AddressTemplate tmpl, string restTail)
        {
            var ret = new List<FindEndPoint>();
            if (foundList != null)
                foreach (var geo in foundList)
                {
                    ResolveRating rating = foundAcronym != null && foundAcronym.Any(s => s.Equals(geo.AcronymName, StringComparison.InvariantCultureIgnoreCase)) ? ResolveRating.Clear : ResolveRating.AcronymNotFound;

                    if (parents == null)
                    {
                        var point = new FindEndPoint() { FoundGeo = geo, ResolveRating = rating, RestTail = restTail };
                        ret.Add(point);
                    }
                    else
                    {
                        foreach (var point in parents)
                        {
                            rating |= EnsureParentChildConsistency(point.FoundGeo, geo);
                            if (rating < ResolveRating.Excess)
                            {
                                ObjGeo actualGeo = null;
                                //51 Признак актуальности - в актуальное , т.е. в 0
                                if (geo.Actual == 51)
                                {
                                    actualGeo = DbHelper.Select<ObjGeo>("select top 1 * from tblKLADR where OriginalCode in (select NewCode from tbl_KLADR_AltNames where OldCode=@code and Lvl=@lvl)"
                                        , DbHelper.CreateParameter("@code", geo.OriginalCode.Substring(0, geo.OriginalCode.Length - 2) + "00")
                                        , DbHelper.CreateParameter("@lvl", (int)geo.Level + 1)).FirstOrDefault();
                                }

                                var newPoint = new FindEndPoint() { FoundGeo = actualGeo ?? geo, ParentEndPoint = point, ResolveRating = rating | point.ResolveRating, RestTail = restTail };
                                ret.Add(newPoint);
                            }
                        }
                    }
                }

            ret.ForEach(point => point.Template = tmpl);
            return ret;
        }

        internal static IEnumerable<FindEndPoint> CloseBldLevelFound(IEnumerable<FindEndPoint> parents, Location loc, string zip)
        {
            var ret = new List<FindEndPoint>();
            var blds = new List<ObjGeo>();
            parents = parents.ToList();
            if (parents.Count() > 0)
                blds = DbHelper.CreateCommand("select * from tblKLADR where GeoType=@lvl5 and ParentCode=@code")
                    .AddParameter("@lvl5", GeoLevelType.Building)
                    .ExpandParameterToList("@code", parents.Select(p => p.FoundGeo.Code)).Select<ObjGeo>().ToList();

            if (!string.IsNullOrEmpty(zip))
            {
                foreach (var bldGeo in blds.Where(bld => bld.Index == zip))
                {
                    var bldRating = loc.BuildingValidate(bldGeo);
                    var point = parents.First(p => p.FoundGeo.Code == bldGeo.ParentCode);
                    var newPoint = new FindEndPoint() { FoundGeo = bldGeo, ParentEndPoint = point, ResolveRating = point.ResolveRating | bldRating };
                    ret.Add(newPoint);
                }
            }
            else
            {
                foreach (var bldGeo in blds.Where(bld => loc.BuildingValidate(bld) == ResolveRating.Clear))
                {
                    var point = parents.First(p => p.FoundGeo.Code == bldGeo.ParentCode);
                    var newPoint = new FindEndPoint() { FoundGeo = bldGeo, ParentEndPoint = point, ResolveRating = point.ResolveRating };
                    ret.Add(newPoint);
                }
            }

            return ret;
        }

        internal static Location ParseLocation(string originalString)
        {
            Location found = new Location();
            StringBuilder b = new StringBuilder(originalString);
            if (b.Length > 0)
            {
                // ищем а/я
                Match m = PostBoxRx.Match(b.ToString());
                if (m.Groups["postbox"].Success)
                {
                    found.Site = m.Groups["postbox"].Value;
                    found.SiteType = SiteType.PostOfficeBox;
                    b.Replace(m.Value, string.Empty);
                }

                // ищем в/ч
                m = MilitaryBaseRx.Match(b.ToString());
                if (m.Groups["milbase"].Success)
                {
                    found.Site = m.Groups["milbase"].Value;
                    found.SiteType = SiteType.MilitaryBase;
                    b.Replace(m.Value, string.Empty);
                }

                string s = SpaceRx.Replace(b.ToString(), " ").Trim();
                b.Length = 0;
                b.Append(s);

                int cursor = 0;
                m = BldRx.Match(b.ToString(cursor, b.Length - cursor));
                if (m.Groups["bld"].Success)
                {
                    found.Building = m.Groups["bld"].Value.ToUpper();
                    for (cursor += m.Value.Length; cursor < b.Length && b[cursor] == ' '; cursor++) ;
                }
                else
                {
                    b.Length = 0;
                }

                m = BlockRx.Match(b.ToString(cursor, b.Length - cursor));
                if (m.Groups["block"].Success)
                {
                    found.Block = m.Groups["block"].Value.Replace(" ", "");//.ToUpper();
                    if (m.Groups["bt"].Success)
                        found.BlockType = m.Groups["bt"].Value;
                    for (cursor += m.Value.Length; cursor < b.Length && b[cursor] == ' '; cursor++) ;
                }

                m = FlatRx.Match(b.ToString(cursor, b.Length - cursor));

                if (m.Groups["flat"].Success)
                {
                    found.Site = m.Groups["flat"].Value;
                    found.SiteType = SiteType.Flat;
                    for (cursor += m.Value.Length; cursor < b.Length && b[cursor] == ' '; cursor++) ;
                    if (m.Groups["flattype"].Success)
                    {
                        string flattype = m.Groups["flattype"].Value;
                        switch (flattype)
                        {
                            case "к":
                            case "ком":
                            case "комн":
                            case "комната":
                                found.SiteType = SiteType.Room;
                                break;
                            case "лит":
                            case "литера":
                                found.SiteType = SiteType.Letter;
                                break;
                        }
                    }
                }
            }

            return found;
        }

        internal static void FinallyCheck(IEnumerable<FindEndPoint> founds, string zip)
        {
            foreach (var candidate in founds.Where(p => p.FoundGeo.Index != zip))
            {
                candidate.ResolveRating |= ResolveRating.WrongZip;
            }

            var checklist = founds.Where(p => p.FoundGeo.Level < GeoLevelType.Street);

            if (checklist.Count() > 0)
            {
                //проверка на случай когда внизу есть улицы, а в исходной строке улицы не было, или парсер не нашёл
                var failCodes = DbHelper.CreateCommand("select distinct ParentCode from tblKLADR where ParentCode=@code and GeoType=@lvl4")
                    .AddParameter("@lvl4", GeoLevelType.Street)
                    .ExpandParameterToList("@code", checklist.Select(p => p.FoundGeo.Code))
                    .SelectScalarList<string>();

                foreach (var candidate in checklist.Where(p => failCodes.Any(s => s == p.FoundGeo.Code)))
                {
                    candidate.ResolveRating |= ResolveRating.Shortage;
                }
            }
        }
    }
}

