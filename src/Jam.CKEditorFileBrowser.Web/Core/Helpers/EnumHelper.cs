using System;
using System.ComponentModel;

namespace Jam.CKEditorFileBrowser.Web.Core.Helpers
{
	public static class EnumHelper
	{
		#region Public Static Methods

		public static string GetDescription<T>(T value)
		{
			if (!typeof(T).IsEnum)
				throw new InvalidOperationException(string.Format("O tipo \"{0}\" não é uma estrutura de enumerador.", typeof(T).FullName));

			if (value == null)
				throw new ArgumentNullException("value", "\"value\" não pode ser nulo.");

			var descriptions = typeof(T)
									.GetField(value.ToString())
									.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return (descriptions.Length != 0 ? ((DescriptionAttribute)descriptions[0]).Description : string.Empty);
		}

		public static T Convert<T>(object value)
		{
			if (!typeof(T).IsEnum)
				throw new InvalidOperationException(string.Format("O tipo \"{0}\" não é uma estrutura de enumerador.", typeof(T).FullName));

			if (value == null)
				throw new ArgumentNullException("value", "\"value\" não pode ser nulo.");

			if (!Enum.IsDefined(typeof(T), value)) return default(T);

			return (T)Enum.Parse(typeof(T), value.ToString());
		}

		#endregion
	}
}