using System;
using System.ComponentModel;
using System.Text;

namespace RF.Geo.BL
{
	public enum GeoLevelType: byte
	{
		/// <summary>
		/// Уровень субъекта РФ
		/// </summary>
        State = 0,

		/// <summary>
		/// Регион/район
		/// </summary>
		Region = 1,

		/// <summary>
		/// Город, крупный населенный пункт
		/// </summary>
        City = 2,

		/// <summary>
		/// Городские и сельские районы, мелкие населенные пункты
		/// </summary>
        Place = 3,

		/// <summary>
		/// Улицы, проспекты и т.п.
		/// </summary>
        Street = 4,

		/// <summary>
		/// Уровень диапозона номеров зданий
		/// </summary>
		Building = 5
	}

    public static class GeoLevelTypeExtension
    {
        //SS    RRR CCC PPP UUUU    DDDD
        //00    000 000 001 1111    1111
        //01    234 567 890 1234    5678
        
        public static int Start(this GeoLevelType lvl)
        {
            switch (lvl)
            {
                case GeoLevelType.State: return 0;
                case GeoLevelType.Region: return 2;
                case GeoLevelType.City: return 5;
                case GeoLevelType.Place: return 8;
                case GeoLevelType.Street: return 11;
                case GeoLevelType.Building: return 15;
            }

            return 0;
        }

        public static int Len(this GeoLevelType lvl)
        {
            switch (lvl)
            {
                case GeoLevelType.State: return 2;
                case GeoLevelType.Region: 
                case GeoLevelType.City: 
                case GeoLevelType.Place: return 3;
                case GeoLevelType.Street: 
                case GeoLevelType.Building: return 4;
            }

            return 0;
        }

        /// <summary>
        /// Вычленяет из кода часть, соответствующую уровню
        /// </summary>
        public static string CodePart(this GeoLevelType lvl, string code)
        {
            StringBuilder sb = new StringBuilder(code);

            if (sb.Length == 19)
            {
                return sb.ToString(lvl.Start(), lvl.Len());
            }

            return string.Empty;
        }

        /// <summary>
        /// обрезает код справа так, чтобы длина оставшейся строки равнялась значащей длине кода соответсвующего уровня
        /// </summary>
        public static string CodeTrim(this GeoLevelType lvl, string code)
        {
            StringBuilder sb = new StringBuilder(code);

            if (sb.Length == 19)
            {
                return sb.ToString(0, lvl.Start() + lvl.Len());
            }

            return string.Empty;
        }
    }
}
