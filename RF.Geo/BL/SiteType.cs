using System;
using System.ComponentModel;

using RF.Common;

namespace RF.Geo.BL
{
	public enum SiteType : byte
	{
		[AcronymDesc("кв.", "Квартира")]
		Flat = 0,

		[AcronymDesc("ком.", "Комната")]
		Room = 1,

		[AcronymDesc("оф.", "Офис")]
		Office = 2,

		[AcronymDesc("лит.", "Литера")]
		Letter = 3,

		[AcronymDesc("а/я", "Аб.Ящик")]
		PostOfficeBox = 5,

		[AcronymDesc("подъезд", "Подъезд")]
		Entrance = 6,

		[AcronymDesc("в/ч", "в/ч")]
		MilitaryBase = 7
	}
}
