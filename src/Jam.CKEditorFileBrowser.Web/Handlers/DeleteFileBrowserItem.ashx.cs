using System;
using System.Text;
using System.Web;
using Jam.CKEditorFileBrowser.Web.Core.Infrastructure;
using log4net;

namespace Jam.CKEditorFileBrowser.Web.Handlers
{
	public class DeleteFileBrowserItem : IHttpHandler
	{
		#region Private Static Read-Only Fields

		private static readonly ILog logger = LogManager.GetLogger(typeof(DeleteFileBrowserItem));

		#endregion

		#region Public Properties

		public bool IsReusable { get { return false; } }

		#endregion

		#region Public Methods

		public void ProcessRequest(HttpContext context)
		{
			this.Delete(context);
		}

		#endregion

		#region Private Methods

		private void Delete(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "text/html";

			if (context.Request.HttpMethod.ToUpper() == "POST")
			{
				if (context.Request["item"] != null)
				{
					try
					{
						if (FileSystemManager.Instance.DeleteItem(context.Request["item"]))
							context.Response.Write("Item removido com sucesso.");
						else
							context.Response.Write("Não foi possível remover o item.");
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message, ex);
						context.Response.Write(string.Format("Ocorreu um erro no momento de remover o item. Erro: \"{0}\".", ex.Message));
					}
				}
				else
					context.Response.Write("Não foi informado o item a ser removido.");
			}
			else
				context.Response.Write("Apenas chamadas via POST.");

			context.Response.End();
		}

		#endregion
	}
}