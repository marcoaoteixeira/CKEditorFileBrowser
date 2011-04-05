using System;
using System.Text;
using System.Web;
using Jam.CKEditorFileBrowser.Web.Core.Infrastructure;
using log4net;

namespace Jam.CKEditorFileBrowser.Web.Handlers
{
	public class CreateFileBrowserNewFolder : IHttpHandler
	{
		#region Private Static Read-Only Fields

		private static readonly ILog logger = LogManager.GetLogger(typeof(CreateFileBrowserNewFolder));

		#endregion

		#region Public Properties

		public bool IsReusable { get { return false; } }

		#endregion

		#region Public Methods

		public void ProcessRequest(HttpContext context)
		{
			this.Create(context);
		}

		#endregion

		#region Private Methods

		private void Create(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "text/plain";

			if (context.Request.HttpMethod.ToUpper() == "POST")
			{
				if (context.Request["relativeFolder"] != null && context.Request["folder"] != null)
				{
					string relativeFolder = context.Request["relativeFolder"];
					string folder = context.Request["folder"];

					try
					{
						FileSystemManager.Instance.CreateFolder(string.Concat(relativeFolder, folder));
						context.Response.Write("Pasta criada com sucesso.");
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message, ex);
						context.Response.Write(string.Format("Erro ao criar a pasta. Erro: \"{0}\".", ex.Message));
					}
				}
				else
					context.Response.Write("Não foi informado a pasta relativa e/ou a nova pasta.");
			}
			else
				context.Response.Write("Apenas chamadas via POST.");

			context.Response.End();
		}

		#endregion
	}
}