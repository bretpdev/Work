using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DocumentProcessing;

namespace UHECORPRT
{
    partial class BatchPrinting
    {
        private void Image(ScriptData file, PrintProcessingData data, object saveAs, bool isCoBorrower)
        {
            DocumentProcessing.ImageFile(saveAs.ToString(), file.DocIdName, data.BF_SSN);
            data.MarkImagingDone(DA, isCoBorrower);
        }
    }
}
