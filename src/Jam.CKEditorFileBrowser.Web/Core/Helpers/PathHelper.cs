using System.IO;

namespace Jam.CKEditorFileBrowser.Web.Core.Helpers
{
	public static class PathHelper
	{
		#region Public Static Methods

		public static string GetDirectoryName(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			int lastPathBackslashPosition = path.Replace("/", "\\").LastIndexOf("\\");
			int pathLength = path.Length;

			if (lastPathBackslashPosition < (pathLength - 1))
				return Path.GetFileName(path);

			return Path.GetFileName(Path.GetDirectoryName(path));
		}

		#endregion
	}
}