using System;
using System.Configuration;
using System.IO;
using System.Web;
using Jam.CKEditorFileBrowser.Web.Core.Helpers;
using log4net;

namespace Jam.CKEditorFileBrowser.Web.Core.Infrastructure
{
	public sealed class FileSystemManager
	{
		#region Singleton

		private static readonly object padlock = new object();
		private static FileSystemManager _instance;
		public static FileSystemManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (_instance == null)
						_instance = new FileSystemManager();

					return _instance;
				}
			}
		}
		private FileSystemManager() { this.Initialize(); }

		#endregion

		#region Private Static Read-Only Fields

		private static readonly ILog logger = LogManager.GetLogger(typeof(FileSystemManager));

		#endregion

		#region Private Constants

		private const string DEFAULT_VIRTUAL_UPLOAD_ROOT_FOLDER = "~/";
		private const string CONFIG_VIRTUAL_UPLOAD_ROOT_FOLDER_KEY = "ckeditorfilebrowser.virtual.upload.root.folder";

		#endregion

		#region Private Fields

		private string _mappedUploadRootFolder;

		#endregion

		#region Public Properties

		public string MappedUploadRootFolder
		{
			get { return this._mappedUploadRootFolder; }
		}

		#endregion

		#region Public Methods

		public DirectoryInfo CreateFolder(string virtualPath)
		{
			try
			{
				string mappedPath = WebPathHelper.ConvertToAbsolutePath(virtualPath);

				if (!mappedPath.StartsWith(this._mappedUploadRootFolder))
					throw new InvalidOperationException("O caminho de destino não é o mesmo que foi definido na aplicação.");

				if (!Directory.Exists(mappedPath))
					return Directory.CreateDirectory(mappedPath);

				return null;
			}
			catch (Exception ex) { logger.Error(ex.Message, ex); throw; }
		}

		public bool DeleteItem(string virtualPath)
		{
			try
			{
				string mappedPath = WebPathHelper.ConvertToAbsolutePath(virtualPath);

				if (!mappedPath.StartsWith(this._mappedUploadRootFolder))
					throw new InvalidOperationException("O caminho de destino não é o mesmo definido na aplicação.");

				if (virtualPath.EndsWith("/"))
					Directory.Delete(mappedPath, true);
				else
					File.Delete(mappedPath);

				return true;
			}
			catch (Exception ex) { logger.Error(ex.Message, ex); throw; }
		}

		public void SavePostedFile(HttpPostedFile file, string virtualPath)
		{
			if (file == null)
				throw new ArgumentNullException("file", "\"file\" não pode ser nulo.");

			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath", "\"virtualPath\" não pode ser nulo.");

			string mappedPath = WebPathHelper.MapPath(virtualPath);

			if (!mappedPath.StartsWith(this._mappedUploadRootFolder))
				throw new InvalidOperationException("O caminho de destino não é o mesmo definido na aplicação.");

			this.CreateFolder(virtualPath);

			string fileLocation = string.Concat(StringHelper.EnsureEndsWith(mappedPath, "\\"), Path.GetFileName(file.FileName));

			file.SaveAs(fileLocation);
		}

		#endregion

		#region Private Methods

		private void Initialize()
		{
			this._mappedUploadRootFolder = WebPathHelper.MapPath(ConfigurationManager.AppSettings.Get(CONFIG_VIRTUAL_UPLOAD_ROOT_FOLDER_KEY) ?? DEFAULT_VIRTUAL_UPLOAD_ROOT_FOLDER);
		}

		#endregion
	}
}