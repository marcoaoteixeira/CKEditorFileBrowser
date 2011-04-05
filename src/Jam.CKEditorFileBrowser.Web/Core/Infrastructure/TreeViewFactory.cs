using System;
using System.IO;
using Jam.CKEditorFileBrowser.Web.Core.Helpers;

namespace Jam.CKEditorFileBrowser.Web.Core.Infrastructure
{
	public static class TreeViewFactory
	{
		#region Public Static Methods

		public static TreeViewNode BuildFrom(string absolutePath)
		{
			if (string.IsNullOrEmpty(absolutePath)) return null;

			string absoluteRootPath = StringHelper.EnsureEndsWith(absolutePath, "\\");

			if (!Directory.Exists(absoluteRootPath))
				throw new DirectoryNotFoundException("Diretório informado para montagem da árvore de arquivos não existe.");

			TreeViewNode root = new TreeViewNode();

			root.Name = PathHelper.GetDirectoryName(absoluteRootPath);
			root.Path = WebPathHelper.ConvertToVirtualPath(absoluteRootPath);
			root.Type = TreeViewNodeType.Root;

			TreeViewFactory.FetchChidrenFolders(absoluteRootPath, root);
			TreeViewFactory.FetchChildrenFiles(absoluteRootPath, root);

			return root;
		}

		#endregion

		#region Private Static Methods

		private static void FetchChidrenFolders(string root, TreeViewNode parent)
		{
			if (string.IsNullOrEmpty(root)) return;

			try
			{
				foreach (DirectoryInfo directory in new DirectoryInfo(root).GetDirectories())
				{
					TreeViewNode node = new TreeViewNode();

					node.Name = StringHelper.EnsureStartsWith(directory.Name, "/");
					node.Path = StringHelper.EnsureEndsWith(WebPathHelper.ConvertToVirtualPath(directory.FullName), "/");
					node.Type = TreeViewNodeType.Folder;

					TreeViewFactory.FetchChidrenFolders(directory.FullName, node);
					
					parent.Children.Add(node);

					TreeViewFactory.FetchChildrenFiles(directory.FullName, node);
				}
			}
			catch (Exception ex) { throw ex; }
		}

		private static void FetchChildrenFiles(string folder, TreeViewNode parent)
		{
			foreach (FileInfo file in new DirectoryInfo(folder).GetFiles())
			{
				parent.Children.Add(new TreeViewNode(file.Name
					, WebPathHelper.ConvertToVirtualPath(file.FullName)
					, TreeViewFactory.GetFileMimeType(file.Extension)
					, parent
					, null
				));
			}
		}

		private static TreeViewNodeType GetFileMimeType(string extension)
		{
			if (extension == null) return TreeViewNodeType.General;

			switch (extension)
			{
				case ".xls":
				case ".xlsx": return TreeViewNodeType.MsExcel;
				case ".ppt":
				case ".pptx": return TreeViewNodeType.MsPowerPoint;
				case ".doc":
				case ".docx": return TreeViewNodeType.MsWord;
				case ".swf": return TreeViewNodeType.AdobeFlash;
				case ".pdf": return TreeViewNodeType.AdobePdf;
				case ".bmp":
				case ".gif":
				case ".jpg":
				case ".jpeg":
				case ".png": return TreeViewNodeType.Picture;
				default: return TreeViewNodeType.General;
			}
		}

		#endregion
	}
}