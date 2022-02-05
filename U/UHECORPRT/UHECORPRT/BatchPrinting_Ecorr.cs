using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace UHECORPRT
{
    partial class BatchPrinting
    {
        private void DoEcorr(ScriptData file, PrintProcessingData data, object saveAs, bool isCoBorrower)
        {
            string ssn;

            if (!isCoBorrower)
            {
                ssn = file.IsEndorser ? data.LetterDataConst.SplitAndRemoveQuotes(",")[file.EndorsersBorrowerSSNIndex.Value] : data.BF_SSN;
            }
            else
            {
                ssn = data.BF_SSN;
            }

            //string ssn = file.IsEndorser ? data.LetterData.SplitAndRemoveQuotes(",")[file.EndorsersBorrowerSSNIndex.Value] : data.BF_SSN;

            DocumentProperties.CorrMethod ecorr = data.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed;
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(saveAs.ToString());
            Uheaa.Common.DocumentProcessing.DocumentProperties docprop = new DocumentProperties(ssn, data.AccountNumber, file.Letter, "UT00204",
                data.EmailAddress, ecorr, path, data.DueDate, data.BillTotalDue, data.BillSeq, data.BillCreateDate);
            docprop.InsertEcorrInformation();
            data.MarkEcorrDone(DA, isCoBorrower);
        }
    }
}
