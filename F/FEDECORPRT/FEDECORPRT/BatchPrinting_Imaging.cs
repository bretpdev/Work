using System;
using System.IO;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace FEDECORPRT
{
    partial class BatchPrinting
    {
        private void Image(ScriptData file, PrintProcessingData data, object saveAs, bool isCoBorrower)
        {
            Console.WriteLine("Imaging Document for PrintProcessingId:{0}", data.PrintProcessingId);
            //DocumentProcessing.ImageFile(saveAs.ToString(), file.DocIdName, data.BF_SSN);
            string fileExtension = saveAs.ToString().Substring(saveAs.ToString().LastIndexOf("."));
            string uniqueId = Guid.NewGuid().ToString();
            string imagingFolder = EnterpriseFileSystem.GetPath("IMAGING");
            string fileName = Path.Combine(imagingFolder, Path.GetFileName(saveAs.ToString()));
            string destination = string.Format("{0}{1}_{2}{3}", imagingFolder, data.BF_SSN, uniqueId, fileExtension);
            string controlFile = string.Format("{0}{1}_{2}.ctl", imagingFolder, data.BF_SSN, uniqueId);
            using (StreamWriter sw = new StreamWriter(controlFile))
            {
                sw.WriteLine(string.Format("~^Folder~{0:MM/dd/yyyy} {1}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{2}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{3}^Attribute~DOC_DATE~STR~{0:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{0:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{0:HH:mm:ss}^Attribute~DESCRIPTION~STR~{0:MM/dd/yyyy} {1}", DateTime.Now, DateTime.Now.TimeOfDay, data.BF_SSN, file.DocIdName));
                sw.WriteLine(string.Format("DesktopDoc~{0}~{4:MM/dd/yyyy} {5}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{1}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{6}^Attribute~DOC_DATE~STR~{4:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{4:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{4:HH:mm:ss}", fileName, data.BF_SSN, uniqueId, fileExtension, DateTime.Now, DateTime.Now.TimeOfDay, file.DocIdName));
            }
            data.MarkImagingDone(DA, isCoBorrower);
        }
    }
}