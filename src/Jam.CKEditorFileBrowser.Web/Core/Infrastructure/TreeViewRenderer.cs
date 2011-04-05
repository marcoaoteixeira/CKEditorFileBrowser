using System.Diagnostics;
using System.IO;
using Jam.CKEditorFileBrowser.Web.Core.Helpers;

namespace Jam.CKEditorFileBrowser.Web.Core.Infrastructure
{
	public static class TreeViewRenderer
	{
		#region Private Constants

		private const string TAB = "\t";
		private const string ROOT_TREE = "<ul class=\"treeview\">";
		private const string FOLDER_TREE = "<ul>";
		private const string ITEM_BEGIN = "<li><span class=\"{0}\"><a href=\"#{1}\">{2}</a></span>";
		private const string ITEM_END = "</li>";
		private const string ROOT_FOLDER_END = "</ul>";

		#endregion

		#region Public Static Methods

		public static void Render(TextWriter writer, TreeViewNode treeViewRoot)
		{
			TreeViewRenderer.Render(writer, new TreeViewNode[] { treeViewRoot }, 0);
		}

		#endregion

		#region Private Static Methods

		private static void Render(TextWriter writer, TreeViewNode[] treeViewNodes, int tabIndex)
		{
			TreeViewRenderer.RenderTabs(writer, tabIndex);

			if (treeViewNodes.Length > 0)
			{	
				writer.WriteLine(treeViewNodes[0].Type == TreeViewNodeType.Root ? ROOT_TREE : FOLDER_TREE);

				foreach (TreeViewNode treeViewNode in treeViewNodes)
				{
					TreeViewRenderer.RenderTabs(writer, tabIndex + 1);

					writer.Write(ITEM_BEGIN
						, EnumHelper.GetDescription<TreeViewNodeType>(treeViewNode.Type)
						, treeViewNode.Path
						, treeViewNode.Name);

					TreeViewRenderer.Render(writer, treeViewNode.Children.ToArray(), tabIndex + 2);

					writer.Write(ITEM_END);

					TreeViewRenderer.RenderTabs(writer, tabIndex + 1);
				}

				writer.Write(ROOT_FOLDER_END);
			}

			TreeViewRenderer.RenderTabs(writer, tabIndex);
		}

		[DebuggerStepThrough]
		private static void RenderTabs(TextWriter writer, int tabIndex)
		{
			for (int i = 0; i < tabIndex; i++)
				writer.Write(TAB);
		}

		#endregion
	}
}