using System;
using System.Diagnostics;
using System.Globalization;

namespace RF.Common
{
	/// <summary>
	/// Набор методов для проверки входных параметров.
	/// </summary>
	/// <remarks>
	/// Пример использования:
	/// <code>
	/// public void SomeMethod(DBObject obj, string id, int percent)
	/// {
	///		Args.IsNotNull(obj, "obj");
	///		Args.IsNotNullOrEmpty(id, "id");
	///		Args.IsTrue(IsValidId(id), "id", "Указан несуществующий ID");
	///		Args.IsInRange(percent, 1, 100, "percent");
	///		...
	/// }
	/// </code>
	/// </remarks>
	public static class Args
	{
		/// <summary>
		/// Проверяет, что значение параметра не null.
		/// </summary>
		/// <param name="argValue">Значение.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <exception cref="ArgumentNullException">Если значение null.</exception>
		[DebuggerStepThrough]
		public static void IsNotNull(object argValue, string argName)
		{
			if (argValue == null || argValue == DBNull.Value)
			{
				throw new ArgumentNullException(argName);
			}
		}

		/// <summary>
		/// Проверяет, что строковое значение параметра не null и не пустая строка.
		/// </summary>
		/// <param name="argValue">Значение.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <exception cref="ArgumentNullException">Если значение null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Если значение пустая строка.</exception>
		[DebuggerStepThrough]
		public static void IsNotNullOrEmpty(string argValue, string argName)
		{
			if (argValue == null)
			{
				throw new ArgumentNullException(argName);
			}
			else if (argValue.Length == 0)
			{
				throw new ArgumentOutOfRangeException(argName);
			}
		}

		/// <summary>
		/// Проверяет, что значение принадлежит диапазону.
		/// </summary>
		/// <param name="argValue">Значение.</param>
		/// <param name="minValue">Нижняя граница диапазона. Не может быть null.</param>
		/// <param name="maxValue">Верхняя граница диапазона. Не может быть null.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <param name="message">Опциональное сообщение об ошибке.</param>
		/// <exception cref="ArgumentNullException">Если minValue или maxValue равно null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Если значение равно null или лежит вне диапазона.</exception>
		[DebuggerStepThrough]
		public static void IsInRange(object argValue, object minValue, object maxValue, string argName, string message)
		{
			IsNotNull(minValue, "minValue");
			IsNotNull(maxValue, "maxValue");

			if (argValue == null ||
				System.Collections.Comparer.Default.Compare(minValue, argValue) > 0 ||
				System.Collections.Comparer.Default.Compare(maxValue, argValue) < 0)
			{
				throw new ArgumentOutOfRangeException(argName, argValue,
					message ?? string.Format(CultureInfo.CurrentCulture, "Значение вне допустимого диапазона [{0};{1}].", minValue, maxValue));
			}
		}

		/// <summary>
		/// Проверяет, что значение принадлежит диапазону.
		/// </summary>
		/// <param name="argValue">Значение.</param>
		/// <param name="minValue">Нижняя граница диапазона. Не может быть null.</param>
		/// <param name="maxValue">Верхняя граница диапазона. Не может быть null.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <exception cref="ArgumentNullException">Если minValue или maxValue равно null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Если значение равно null или лежит вне диапазона.</exception>
		[DebuggerStepThrough]
		public static void IsInRange(object argValue, object minValue, object maxValue, string argName)
		{
			IsInRange(argValue, minValue, maxValue, argName, null);
		}

		/// <summary>
		/// Проверяет, что истинно предположение относительно параметра.
		/// </summary>
		/// <param name="condition">Условное выражение.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <remarks>
		/// Пример использования:
		/// <code>
		/// public void SomeMethod(string id)
		/// {
		///		Args.IsTrue(IsValidId(id), "id");
		///		...
		/// }
		/// </code>
		/// </remarks>
		[DebuggerStepThrough]
		public static void IsTrue(bool condition, string argName)
		{
			if (!condition)
			{
				throw new ArgumentException(string.Empty, argName);
			}
		}

		/// <summary>
		/// Проверяет, что истинно предположение относительно параметра.
		/// </summary>
		/// <param name="condition">Условное выражение.</param>
		/// <param name="argName">Имя параметра.</param>
		/// <param name="message">Сообщение об ошибке.</param>
		/// <remarks>
		/// Пример использования:
		/// <code>
		/// public void SomeMethod(string id)
		/// {
		///		Args.IsTrue(IsValidId(id), "id", "Указан несуществующий ID");
		///		...
		/// }
		/// </code>
		/// </remarks>
		[DebuggerStepThrough]
		public static void IsTrue(bool condition, string argName, string message)
		{
			if (!condition)
			{
				throw new ArgumentException(message, argName);
			}
		}
	}
}
