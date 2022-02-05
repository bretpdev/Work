using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace UHECORPRT
{
    public class ScriptData
    {
        public int ScriptDataId { get; set; }
        public string ScriptID { get; set; }
        public string SourceFile { get; set; }
        public string Letter { get; set; }
        public string DocIdName { get; set; }
        public string FileHeader { get; set; }
        public int StateIndex { get; set; }
        public int AccountNumberIndex { get; set; }
        public int CostCenterCodeIndex { get; set; }
        public bool ProcessAllFiles { get; set; }
        public bool IsEndorser { get; set; }
        public int? EndorsersBorrowerSSNIndex { get; set; }
        public int Priority { get; set; }
        public bool AddBarCodes { get; set; }
        public int? BillDueDateIndex { get; set; }
        public int? TotalDueIndex { get; set; }
        public int? BillSeqIndex { get; set; }
        public int? BillCreateDateIndex { get; set; }
        public List<ArcInformation> Arcs { get; set; }
        public List<PrintProcessingData> LetterDataForBorrowers { get; set; }
        public List<PrintProcessingData> LetterDataForCoBorrowers { get; set; }
        public string BarcodeHeader { get; set; }
        public string Recipient { get; set; }
        public bool CheckForCoBorrower { get; set; }
        public string FileHeaderConst { get; set; }
        public int? BarcodeOffset { get; set; }

        public void GetArcData(DataAccess DA)
        {
            Arcs = ArcInformation.Populate(DA, this.ScriptDataId);
        }

        public void SetLastRun(DataAccess DA)
        {
            DA.SetLastProcessed(this.ScriptDataId);
        }


    }
}
