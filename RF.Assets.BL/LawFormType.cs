using System;
using System.ComponentModel;

namespace RF.BL.Model.Enums
{
	/// <summary>
	/// нПЦЮМХГЮЖХНММН-ОПЮБНБЮЪ ТНПЛЮ
	/// </summary>
	public enum LawFormType : byte
	{
		[Description(" ")]
		None = 0,

		[Description("оанчк")]
		PBOUL = 1,

		[Description("хо")]
		IP = 2,

		[Description("гюн")]
		ZAO = 3,

		[Description("нннх")]
		OOOI = 4,

		[Description("нюн")]
		OAO = 5,

		[Description("во")]
		CHP = 6,

		[Description("цсо")]
		GUP = 7,

		[Description("ннн")]
		OOO = 8,

		[Description("яннн")]
		SOOO = 9,

		[Description("лсо")]
		MUP = 10,

		[Description("йр")]
		KT = 11,

		[Description("нцсо")]
		OGUP = 12,

        [Description("ло")]
	    MP = 13,

		[Description("он")]
	    PO = 14,

		[Description("мон")]
	    NPO = 15,

        [Description("оогяй")]
        PPZSK = 16,

        [Description("рнн")]
        TOO = 17,

        [Description("тцсо")]
        FGUP = 18,

        [Description("яой")]
        SPK = 19,

        [Description("яойу")]
        SPKH = 20,

        [Description("юойт гюн")]
        APKFZAO = 21,

        [Description("гюмон")]
        ZANPO = 22,

		[Description("тцс")]
		FGU = 23,

		[Description("гюнП")]
		ZAOR = 24,

		[Description("яуой")]
		SHPK = 25,

		[Description("гюн мон")]
		ZAONPO = 26,

		[Description("йту")]
		KFH = 27,

		[Description("тХГ.КХЖН")]
		Person = 28,

		[Description("но")]
		SD = 29
	}
}