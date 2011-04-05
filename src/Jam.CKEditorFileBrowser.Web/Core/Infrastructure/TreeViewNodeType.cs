using System;
using System.ComponentModel;

namespace Jam.CKEditorFileBrowser.Web.Core.Infrastructure
{
	[Flags]
	public enum TreeViewNodeType : int
	{
		[Description("root")]
		Root = 0,
		[Description("folder")]
		Folder = 1,
		[Description("file")]
		General = 2,
		[Description("file ms-excel")]
		MsExcel = 4,
		[Description("file ms-powerpoint")]
		MsPowerPoint = 8,
		[Description("file ms-word")]
		MsWord = 16,
		[Description("file adobe-flash")]
		AdobeFlash = 32,
		[Description("file adobe-pdf")]
		AdobePdf = 64,
		[Description("file picture")]
		Picture = 128
	}
}