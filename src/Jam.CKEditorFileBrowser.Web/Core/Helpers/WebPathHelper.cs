using System;
using System.Web;

namespace Jam.CKEditorFileBrowser.Web.Core.Helpers
{
	public static class WebPathHelper
	{
		#region Private Constants

		private const string SERVER_VARIABLE_APPL_PHYSICAL_PATH_KEY = "APPL_PHYSICAL_PATH";

		#endregion

		#region Private Static Read-Only Fields

		private static readonly string _applicationPhysicalPath;
		private static readonly string _applicationVirtualPath;

		#endregion

		#region Static Constructors

		static WebPathHelper()
		{
			_applicationPhysicalPath = HttpContext.Current.Request.ServerVariables[SERVER_VARIABLE_APPL_PHYSICAL_PATH_KEY];
			_applicationVirtualPath = HttpContext.Current.Request.ApplicationPath;
		}

		#endregion

		#region Public Static Methods

		public static string ConvertToVirtualPath(string absolutePath)
		{
			if (!WebPathHelper.StartsWithWebAppAbsolutePath(absolutePath))
				throw new InvalidOperationException("O caminho passado não pertence ao domínio da aplicação.");

			return absolutePath
				.Replace(WebPathHelper.GetApplicationPhysicalPath(), WebPathHelper.GetApplicationVirtualPath())
				.Replace("\\", "/");
		}

		public static string ConvertToAbsolutePath(string virtualPath)
		{
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath", "\"virtualPath\" não pode ser nulo.");

			return WebPathHelper.MapPath(virtualPath.Replace("\\", "/"));
		}

		public static string MapPath(string virtualPath)
		{
			return HttpContext.Current.Server.MapPath(virtualPath);
		}

		#endregion

		#region Private Static Methods

		private static bool StartsWithWebAppAbsolutePath(string path)
		{
			return (!string.IsNullOrEmpty(path) &&
					path.StartsWith(WebPathHelper._applicationPhysicalPath, StringComparison.InvariantCultureIgnoreCase));
		}

		private static string GetApplicationPhysicalPath()
		{
			return _applicationPhysicalPath;
		}

		private static string GetApplicationVirtualPath()
		{
			return _applicationVirtualPath;
		}

		#endregion
	}
}