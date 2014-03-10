using System;
using System.ComponentModel;

namespace RF.Common.Security
{
	/// <summary>
	/// Типы политик. Используется при назначении политик доступа.
	/// </summary>
    public enum PolicyType
    {
        [Description("Не задан")]
        NotSet = 0,

		/// <summary>
		/// Политика данного типа ограничивает доступ к отчетам
		/// </summary>
        [Description("Политика для отчётов")]
        Reports = 1,

        [Description("Политика для служб")]
        Services = 2
    }
}