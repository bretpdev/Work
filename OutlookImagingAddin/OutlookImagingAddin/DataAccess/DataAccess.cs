using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OutlookImagingAddin
{
    class DataAccess
    {
        readonly ProcessLogRun PLR;
        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
            CornerstoneDocuments = PLR.LDA.ExecuteList<Document>("docid.GetDocIds", DataAccessHelper.Database.Cls).Result;
            UheaaDocuments = PLR.LDA.ExecuteList<Document>("docid.GetDocIds", DataAccessHelper.Database.Uls).Result;
        }

        public List<Document> CornerstoneDocuments { get; private set; }
        public List<Document> UheaaDocuments { get; private set; }
    }
}
