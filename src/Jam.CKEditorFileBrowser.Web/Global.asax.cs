using System;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace Jam.CKEditorFileBrowser.Web
{
	public class Global : System.Web.HttpApplication
	{
		#region Protected Methods

		protected void Application_Start(object sender, EventArgs e)
		{
			this.ConfigureLog4Net();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}

		#endregion

		#region Private Methods

		private void ConfigureLog4Net()
		{
			XmlConfigurator.Configure();
		}

		#endregion
	}
}