using System;
using System.Text;
using System.Web;
using Jam.CKEditorFileBrowser.Web.Core.Infrastructure;
using log4net;

namespace Jam.CKEditorFileBrowser.Web.Handlers
{
	public class RenderFileBrowserTreeView : IHttpHandler
	{
		#region Private Static Read-Only Fields

		private static readonly ILog logger = LogManager.GetLogger(typeof(RenderFileBrowserTreeView));

		#endregion

		#region Public Properties

		public bool IsReusable { get { return false; } }

		#endregion

		#region Public Methods

		public void ProcessRequest(HttpContext context)
		{
			this.Build(context);
		}

		#endregion

		#region Private Methods

		private void Build(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "text/html";

			try
			{
				TreeViewNode root = TreeViewFactory.BuildFrom(FileSystemManager.Instance.MappedUploadRootFolder);
				TreeViewRenderer.Render(context.Response.Output, root);
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message, ex);
				context.Response.Write(string.Format("Ocorreu um erro ao construir a árvore de arquivos. Erro: \"{0}\".", ex.Message));
			}

			context.Response.End();
		}

		#endregion
	}
}