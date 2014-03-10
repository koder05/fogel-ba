using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.Common;

namespace RF.Geo.BL
{
    [Flags]
    public enum ResolveRating
    {
        /// <summary>
        /// Ништяк
        /// </summary>
        Clear = 0,
        /// <summary>
        /// Акроним в исходной строке не идентифицирован
        /// </summary>
        AcronymNotFound = 0x100,
        BldExcess = 0x200,
        BldNomatch = 0x400,
        /// <summary>
        /// В исходной строке не хватает предыдущего уровня
        /// </summary>
        Shortage = 0x1000,
        /// <summary>
        /// В исходной строке лишняя информация (лишний предыдущий уровень например)
        /// </summary>
        Excess = 0x2000,
        /// <summary>
        /// Жопа
        /// </summary>
        NoMatch = 0x10000,
        /// <summary>
        /// Индекс не тот, что в исходной строке
        /// </summary>
        WrongZip = 0x100000
    }
}
