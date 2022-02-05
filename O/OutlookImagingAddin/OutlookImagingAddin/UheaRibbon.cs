using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using stdole;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Office = Microsoft.Office.Core;

namespace OutlookImagingAddin
{
    [ComVisible(true)]
    public class UheaRibbon : Office.IRibbonExtensibility
    {
        public void ImageEmailButton_Clicked(IRibbonControl control)
        {
            var inspector = control.Context as Inspector;
            var explorer = control.Context as Explorer;
            var mailItem = inspector?.CurrentItem as MailItem;
            if (mailItem == null && explorer?.Selection?.Count > 0)
                mailItem = explorer?.Selection[1];
            if (mailItem == null)
            {
                Dialog.Def.Ok("Please open an email in your viewing window.");
                return;
            }

            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun plr = new ProcessLogRun("OUTIMAGADD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), 
                DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            using (var form = new ImagingForm(plr, mailItem))
                form.ShowDialog();

            plr.LogEnd();
        }

        public Bitmap ImageEmailButton_GetImage(IRibbonControl control)
        {
            return Properties.Resources.imaging;
        }

        private Office.IRibbonUI ribbon;

        public UheaRibbon()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("OutlookImagingAddin.UheaRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
