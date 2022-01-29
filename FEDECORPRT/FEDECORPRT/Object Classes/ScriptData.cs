using System.Collections.Generic;

namespace FEDECORPRT
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
        public bool DoNotProcessEcorr { get; set; }
        public bool CheckForCoBorrower { get; set; } //ADDED
        public bool AddBarCodes { get; set; }
        public int? BillDueDateIndex { get; set; }
        public int? TotalDueIndex { get; set; }
        public int? BillSeqIndex { get; set; }
        public int? BillCreateDateIndex { get; set; }
        public List<ArcInformation> Arcs { get; set; }
        public List<PrintProcessingData> LetterDataForBorrowers { get; set; }
        public List<PrintProcessingData> LetterDataForCoBorrowers { get; set; }
        public string BarcodeHeader { get; set; }
        public int? BarcodeOffset { get; set; }
        public string FileHeaderConst { get; set; }
        private DataAccess DA {get;set;}

        public void GetArcData(DataAccess da)
        {
            Arcs = ArcInformation.Populate(this.ScriptDataId, da);
            DA = da;
        }

        public void SetLastRun()
        {
            DA.SetLastProcessed(this.ScriptDataId);
        }


    }
}