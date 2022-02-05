using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace IMGHISTFED
{
    public partial class ActivityHistoryReport : FedBatchScript
    {
        //Recovery value is the borrower counter from the R2 SAS file.

        //End-of-job fields are used as both array initializers and dictionary keys, so define them in consts.
        public const string EOJ_TOTAL_FROM_R2 = "Total number of borrowers in the R2 file";
        public const string EOJ_TOTAL_FROM_R3 = "Total number of borrowers in the R3 file";
        public const string EOJ_TOTAL_FROM_R4 = "Total number of borrowers in the R4 file";
        public const string EOJ_TOTAL_FROM_R6 = "Total number of borrowers in the R6 file";
        public const string EOJ_PROCESSED_FROM_R2 = "Total number of borrowers processed in the R2 file";
        public const string EOJ_PROCESSED_FROM_R3 = "Total number of borrowers processed in the R3 file";
        public const string EOJ_PROCESSED_FROM_R4 = "Total number of borrowers processed in the R4 file";
        public const string EOJ_PROCESSED_FROM_R6 = "Total number of borrowers processed in the R6 file";
        public const string ERR_ErrorInProcessing = "There was an error in Processing";
        public static readonly string[] EOJ_FIELDS = { EOJ_TOTAL_FROM_R2, EOJ_TOTAL_FROM_R3, EOJ_TOTAL_FROM_R4, EOJ_TOTAL_FROM_R6, EOJ_PROCESSED_FROM_R2, EOJ_PROCESSED_FROM_R3, EOJ_PROCESSED_FROM_R4, EOJ_PROCESSED_FROM_R6, ERR_ErrorInProcessing };

        private const string SAS_JOB = "Activity History - Imaging FED";
        private List<string> files;
        private int Max = 0;
        private ActivityForm ActForm;
        private IEnumerable<Demographic> Demographics;
        private IEnumerable<Activity> Activities;
        private IEnumerable<Deferment> Deferments;
        private IEnumerable<EndorserDemographic> EndorserDemo;
        private IEnumerable<RepaymentInfo> Repayment;
        public bool ShouldDelete = false;
        private RFileParseResults<Demographic> R2;
        private RFileParseResults<Activity> R3;
        private RFileParseResults<Deferment> R4;
        private RFileParseResults<EndorserDemographic> R6;
        private RFileParseResults<RepaymentInfo> R7;

        public enum FileErrorType
        {
            Missing,
            Empty,
            FileFormat,
            Ok
        }

        public ActivityHistoryReport(ReflectionInterface ri)
            : base(ri, "IMGHISTFED", "ERR_BU35", "EOJ_BU35", EOJ_FIELDS)
        {
        }

        public override void Main()
        {
            RI.LogOut();
            DisplayForm();
            Application.Exit();
        }

        public void DisplayForm()
        {
            ActForm = new ActivityForm(ScriptId, this);
            ActForm.ShowDialog();
        }

    }
}
