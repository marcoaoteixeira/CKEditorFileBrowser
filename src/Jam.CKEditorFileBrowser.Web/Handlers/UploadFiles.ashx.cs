using System.Text;
using System.Web;
using Jam.CKEditorFileBrowser.Web.Core.Infrastructure;
using log4net;
using System;

namespace Jam.CKEditorFileBrowser.Web.Handlers
{
	public class UploadFiles : IHttpHandler
	{
		#region Private Static Read-Only Fields

		private static readonly ILog logger = LogManager.GetLogger(typeof(UploadFiles));

		#endregion

		#region Public Properties

		public bool IsReusable { get { return false; } }

		#endregion

		#region Public Methods

		public void ProcessRequest(HttpContext context)
		{
			this.Upload(context);
		}

		#endregion

		#region Private Methods

		private void Upload(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "text/html";

			if (context.Request.HttpMethod.ToUpper() == "POST")
			{
				if (context.Request.Files.Count > 0 && context.Request["folder"] != null)
				{
					HttpPostedFile file = context.Request.Files["Filedata"];
					string folder = context.Request["folder"];

					try
					{
						FileSystemManager.Instance.SavePostedFile(file, folder);
						context.Response.Write(file.FileName);
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message, ex);
						context.Response.Write(string.Format("Ocorreu um erro ao salvar o(s) arquivo(s). Erro: \"{0}\".", ex.Message));
					}
				}
				else
					context.Response.Write("Não foi informado arquivos para upload ou a pasta de destino.");
			}
			else
				context.Response.Write("Apenas chamadas via POST.");

			context.Response.End();
		}

		#endregion
	}
}