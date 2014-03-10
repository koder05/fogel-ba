using System;

namespace RF.Reporting
{
	/// <summary>
	/// »нтерфейс, определ€ющий, какие типы объектов и их свойства нужно экспортировать
	/// </summary>
	public interface IExportConstraint
	{
		string[] GetExportProperties(Type type);
	}
}