using iText.Html2pdf;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace OutlookImagingAddin
{
    class PdfHelper
    {
        const string htmlScaffold = "<html><body>{0}</body></html>";
        const string br = "<br />";
        public void ConvertMsg(MailItem msg, string outputPath)
        {
            using (FileStream outputPdfStream = new FStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                string html = msg.HTMLBody;
                if (string.IsNullOrWhiteSpace(html))
                    html = string.Format(htmlScaffold, msg.Body.Replace("\r\n", br).Replace("\r", br).Replace("\n", br));
                var prop = new ConverterProperties();
                
                HtmlConverter.ConvertToPdf(html, outputPdfStream);
            }
        }
    }
}
