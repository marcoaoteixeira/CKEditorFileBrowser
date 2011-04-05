using System;

namespace Jam.CKEditorFileBrowser.Web.Core.Helpers
{
	public static class StringHelper
	{
		#region Public Static Methods

		public static string EnsureStartsWith(string source, string startsWith)
		{
			if (source == null) return startsWith;
			if (startsWith == null) return source;

			return (!source.StartsWith(startsWith) ? string.Concat(startsWith, source) : source);
		}

		public static string EnsureEndsWith(string source, string endsWith)
		{
			if (source == null) return endsWith;
			if (endsWith == null) return source;

			return (!source.EndsWith(endsWith) ? string.Concat(source, endsWith) : source);
		}

		public static bool Contains(string source, string compare)
		{
			return StringHelper.Contains(source, compare, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool Contains(string source, string compare, StringComparison stringComparison)
		{
			if (source == null || compare == null) return false;

			return (source.IndexOf(compare, stringComparison) > -1);
		}

		

		#endregion
	}
}