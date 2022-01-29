using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CFSARPTFED
{
    public class CreateFSAReportsFed : FedBatchScript
    {
        //This is needed to be passed to FedBatchScript Constructor.
        private static readonly string[] EojFields = { };

        public CreateFSAReportsFed(ReflectionInterface ri)
            : base(ri, "CFSARPTFED", "ERR_BU35", "EOJ_BU35", EojFields)
        {
            StartupMessage("This script will use various SAS files to create reports to be sent to FSA.  CLick OK to continue, or Cancel to end the script.");
            //Gather all of the reports from the database.
            List<ReportData> reports = DataAccess.GetReportData();
            ReportData report = new ReportData();
            using (Mode selection = new Mode(reports, report))
            {
                if (selection.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();
            }

            //The Report Name will not be null if the user selected and indivual report.  If they selected to run all then it will be empty.
            if (!report.ReportName.IsNullOrEmpty())
            {
                CreateTheReport(reports.Where(p => p.ReportName.Contains(report.ReportName)).Single());
            }
            else
            {
                //Always process Monthly reports, Depending on the month we will want quarterly reports.
                //When this is run all of the data is for the previous month.
                foreach (ReportData item in reports.Where(p => p.Occurance.Contains("Monthly") || (p.Occurance.Contains("Quarterly") && DateTime.Now.Month.IsIn(2, 5, 8, 11))))
                {
                    CreateTheReport(item);
                }
            }

            ProcessingComplete();
        }

        private void CreateTheReport(ReportData reportInfo)
        {
            //Create the report based on the ReportId.
            switch (reportInfo.ReportId)
            {
                case 1:
                    new CornerStoneRehabRecalledLoansFromDmcs(reportInfo,Err).CreateTheReport();
                    break;
                case 2:
                    new CornerStoneBankruptcyAgingReport(reportInfo,Err).CreateTheReport();
                    break;
                case 3:
                    new CornerStoneBankruptcyReport(reportInfo,Err).CreateTheReport();
                    break;
                case 4:
                    new CornerStoneDischargeReports(reportInfo, Err).CreateTheReport();
                    break;
                case 5:
                    new CornerStoneDischargeReports(reportInfo, Err).CreateTheReport();
                    break;
                case 6:
                    new CornerStoneLoansConsolidatedReport(reportInfo,Err).CreateTheReport();
                    break;
                case 7:
                    new CornerStoneLoansRehabilitatedReport(reportInfo, Err).CreateTheReport();
                    break;
                case 8:
                    new CornerStoneLoansTransferredToDMCSReport(reportInfo,Err).CreateTheReport();
                    break;
                case 9:
                    new CornerStoneLoansTransferedToFromTpd(reportInfo,Err).CreateTheReport();
                    break;
                case 10:
                    new CornerStoneMonthlyIbrTracking(reportInfo,Err).CreateTheReport();
                    break;
                case 13:
                    new CornerStoneQuarterlyReviewRandomSamples(reportInfo,Err).CreateTheReport();
                    break;
            }
        }
    }
}
