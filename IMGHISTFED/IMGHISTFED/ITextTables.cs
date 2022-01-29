using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace IMGHISTFED
{
    class ITextTables
    {
        public Font Font10 { get; set; }
        public Font Font10b { get; set; }
        public Font FontLargeb { get; set; }
        private EndOfJobReport Eoj { get; set; }

        public ITextTables(EndOfJobReport Eoj)
        {
            var font10 = FontFactory.GetFont("ARIAL", 8);
            var font10b = FontFactory.GetFont("ARIAL", 8);
            var fontLargeb = FontFactory.GetFont("ARIAL", 12);
            font10b.SetStyle("bold");
            fontLargeb.SetStyle("bold");

            Font10 = font10;
            Font10b = font10b;
            FontLargeb = fontLargeb;
            this.Eoj = Eoj;
        }

        private void CreateHeaderCell(string colName, Font font, PdfPTable table, int colspan = 1, int horizontalAlignment = Element.ALIGN_CENTER)
        {
            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddHeader(new PdfPHeaderCell());
            cell.HorizontalAlignment = horizontalAlignment; //center
            cell.VerticalAlignment = Element.ALIGN_CENTER;//center
            //cell.BackgroundColor = new BaseColor(211, 211, 211);
            cell.Phrase = new Phrase(new Chunk(colName, font));

            if (colspan != 1)
            {
                cell.Colspan = colspan;
            }

            table.AddCell(cell);
        }

        private void CreateRowCell(object value, Font font, PdfPTable table, int colspan = 1, int horizontalAlignment = Element.ALIGN_CENTER)
        {
            if(value == null)
            {
                value = "";
            }

            var cell = new PdfPCell(new Phrase(new Chunk(value.ToString(), font)));
            cell.Border = Rectangle.NO_BORDER;
            //decimal val = 0;
            //if (decimal.TryParse(value.ToString().Replace("$", ""), out val))
            //    cell.HorizontalAlignment = Element.ALIGN_RIGHT;//Align right
            //else
            cell.HorizontalAlignment = horizontalAlignment;


            cell.VerticalAlignment = 2;//center

            if (colspan != 1)
            {
                cell.Colspan = colspan;
            }

            table.AddCell(cell);
        }

        public PdfPTable GetDemosTable(string ssn, string name, IEnumerable<Demographic> borrowerDemographics)
        {
            //Create demographics table
            PdfPTable tableDemos = new PdfPTable(6);
            tableDemos.SetWidths(new float[] { 1f, 1f, 1f, 3f, 3f, 1f });
            tableDemos.HeaderRows = 3;
            tableDemos.Summary = "Address/Phone/Email History";

            CreateHeaderCell("Address/Phone/Email History", FontLargeb, tableDemos, 6);
            tableDemos.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableDemos, 6, Element.ALIGN_LEFT);
            tableDemos.CompleteRow();

            CreateHeaderCell("Date Verified", Font10b, tableDemos);
            CreateHeaderCell("Time Verified", Font10b, tableDemos);
            CreateHeaderCell("Demo Type", Font10b, tableDemos);
            CreateHeaderCell("Previous Demographics", Font10b, tableDemos);
            CreateHeaderCell("Current Demographics", Font10b, tableDemos);
            CreateHeaderCell("Current Validity", Font10b, tableDemos);
            tableDemos.CompleteRow();

            foreach (var demos in borrowerDemographics)
            {
                CreateRowCell(demos.Date, Font10, tableDemos);
                CreateRowCell(demos.Time, Font10, tableDemos);
                CreateRowCell(demos.Type, Font10, tableDemos);
                CreateRowCell(demos.Value, Font10, tableDemos, 1, Element.ALIGN_LEFT);
                CreateRowCell(demos.Value2, Font10, tableDemos, 1, Element.ALIGN_LEFT);
                CreateRowCell(demos.Validity, Font10, tableDemos);
            }

            return tableDemos;
        }

        public PdfPTable GetActivityTable(string ssn, string name, Dictionary<string, IEnumerable<Activity>> orderedActivities)
        {
            PdfPTable tableActivity = new PdfPTable(5);
            tableActivity.SetWidths(new float[] { 1f, 1f, 2f, 4f, 1f });
            tableActivity.HeaderRows = 3;
            tableActivity.Summary = "Activity History";

            CreateHeaderCell("Activity History", FontLargeb, tableActivity, 5);
            tableActivity.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableActivity, 5, Element.ALIGN_LEFT);
            tableActivity.CompleteRow();

            CreateHeaderCell("Date", Font10b, tableActivity);
            CreateHeaderCell("Time", Font10b, tableActivity);
            CreateHeaderCell("Activity Description", Font10b, tableActivity);
            CreateHeaderCell("Comment", Font10b, tableActivity);
            CreateHeaderCell("Recipient", Font10b, tableActivity);
            tableActivity.CompleteRow();

            if (orderedActivities.ContainsKey(ssn))
            {
                foreach (var activity in orderedActivities[ssn])
                {
                    CreateRowCell(activity.Date, Font10, tableActivity);
                    CreateRowCell(activity.Time, Font10, tableActivity);
                    CreateRowCell(activity.Description, Font10, tableActivity, 1, Element.ALIGN_LEFT);
                    CreateRowCell(activity.Comment, Font10, tableActivity, 1, Element.ALIGN_LEFT);
                    CreateRowCell(activity.Recipient, Font10, tableActivity);
                }

                lock (ActivityHistoryReport.processingLock)
                {
                    Eoj.Counts[ActivityHistoryReport.EOJ_TOTAL_FROM_R3].Increment();
                    Eoj.Counts[ActivityHistoryReport.EOJ_PROCESSED_FROM_R3].Increment();
                }
            }
            else
            {
                CreateRowCell("No History Found", Font10, tableActivity, 5);
            }

            return tableActivity;
        }

        public PdfPTable GetDefermentForbearanceTable(string ssn, string name, Dictionary<string, IEnumerable<Deferment>> orderedDeferments)
        {
            PdfPTable tableDefermentForbearance = new PdfPTable(5);
            tableDefermentForbearance.SetWidths(new float[] { 1f, 1f, 1f, 1f, 2f });
            tableDefermentForbearance.HeaderRows = 3;
            tableDefermentForbearance.Summary = "Deferment/Forbearance History";

            CreateHeaderCell("Deferment/Forbearance History", FontLargeb, tableDefermentForbearance, 5);
            tableDefermentForbearance.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableDefermentForbearance, 5, Element.ALIGN_LEFT);
            tableDefermentForbearance.CompleteRow();

            CreateHeaderCell("Loan Program", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Disbursement Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Start Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("End Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Type", Font10b, tableDefermentForbearance);
            tableDefermentForbearance.CompleteRow();

            if (orderedDeferments.ContainsKey(ssn))
            {
                foreach (var deferment in orderedDeferments[ssn])
                {
                    CreateRowCell(deferment.LoanProgram, Font10, tableDefermentForbearance);
                    CreateRowCell(deferment.DisbursementDate, Font10, tableDefermentForbearance);
                    CreateRowCell(deferment.Begin, Font10, tableDefermentForbearance);
                    CreateRowCell(deferment.End, Font10, tableDefermentForbearance);
                    CreateRowCell(deferment.Type, Font10, tableDefermentForbearance, 1, Element.ALIGN_LEFT);
                }

                lock (ActivityHistoryReport.processingLock)
                {
                    Eoj.Counts[ActivityHistoryReport.EOJ_TOTAL_FROM_R4].Increment();
                    Eoj.Counts[ActivityHistoryReport.EOJ_PROCESSED_FROM_R4].Increment();
                }
            }
            else
            {
                CreateRowCell("No History Found", Font10, tableDefermentForbearance, 5);
            }

            return tableDefermentForbearance;
        }

        public PdfPTable GetRepaymentTable(string ssn, string name, Dictionary<string, IEnumerable<RepaymentInfo>> orderedRepayments)
        {
            PdfPTable tableRepayments = new PdfPTable(9);
            tableRepayments.SetWidths(new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f });
            tableRepayments.HeaderRows = 3;
            tableRepayments.Summary = "Repayment Schedule History";

            CreateHeaderCell("Repayment Schedule History", FontLargeb, tableRepayments, 9);
            tableRepayments.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableRepayments, 9, Element.ALIGN_LEFT);
            tableRepayments.CompleteRow();

            CreateHeaderCell("Next Payment Due", Font10b, tableRepayments);
            CreateHeaderCell("Loan Program", Font10b, tableRepayments);
            CreateHeaderCell("Disbursement Date", Font10b, tableRepayments);
            CreateHeaderCell("Status", Font10b, tableRepayments);
            CreateHeaderCell("Type", Font10b, tableRepayments);
            CreateHeaderCell("First Due Date", Font10b, tableRepayments);
            CreateHeaderCell("Gradation Seq.", Font10b, tableRepayments);
            CreateHeaderCell("Term", Font10b, tableRepayments);
            CreateHeaderCell("Payment", Font10b, tableRepayments);
            tableRepayments.CompleteRow();

            if (orderedRepayments.ContainsKey(ssn))
            {
                foreach (var repayment in orderedRepayments[ssn])
                {
                    CreateRowCell(repayment.NextPaymentDue, Font10, tableRepayments);
                    CreateRowCell(repayment.LoanProgram, Font10, tableRepayments);
                    CreateRowCell(repayment.DisbursementDate, Font10, tableRepayments);
                    CreateRowCell(repayment.ScheduleStatus, Font10, tableRepayments);
                    CreateRowCell(repayment.ScheduleType, Font10, tableRepayments);
                    CreateRowCell(repayment.ScheduleFirstDueDate, Font10, tableRepayments);
                    CreateRowCell(repayment.GradationSequence, Font10, tableRepayments);
                    CreateRowCell(repayment.Term, Font10, tableRepayments);
                    CreateRowCell(repayment.Payment, Font10, tableRepayments);
                }
            }
            else
            {
                CreateRowCell("No History Found", Font10, tableRepayments, 9);
            }

            return tableRepayments;
        }

        public PdfPTable GetEndorserDemosTable(string ssn, string name, string endorserSsn, string endorserName, IEnumerable<EndorserDemographic> borrowerDemographics)
        {
            //Create demographics table
            PdfPTable tableDemos = new PdfPTable(6);
            tableDemos.SetWidths(new float[] { 1f, 1f, 1f, 3f, 3f, 1f });
            tableDemos.HeaderRows = 4;
            tableDemos.Summary = "Address/Phone/Email History";

            CreateHeaderCell("Address/Phone/Email History", FontLargeb, tableDemos, 6);
            tableDemos.CompleteRow();

            CreateHeaderCell($"Endorser: {endorserSsn} {endorserName}", Font10, tableDemos, 6, Element.ALIGN_LEFT);
            tableDemos.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableDemos, 6, Element.ALIGN_LEFT);
            tableDemos.CompleteRow();

            CreateHeaderCell("Date Verified", Font10b, tableDemos);
            CreateHeaderCell("Time Verified", Font10b, tableDemos);
            CreateHeaderCell("Demo Type", Font10b, tableDemos);
            CreateHeaderCell("Previous Demographics", Font10b, tableDemos);
            CreateHeaderCell("Current Demographics", Font10b, tableDemos);
            CreateHeaderCell("Current Validity", Font10b, tableDemos);
            tableDemos.CompleteRow();

            foreach (var demos in borrowerDemographics)
            {
                CreateRowCell(demos.Date, Font10, tableDemos);
                CreateRowCell(demos.Time, Font10, tableDemos);
                CreateRowCell(demos.Type, Font10, tableDemos);
                CreateRowCell(demos.Value, Font10, tableDemos);
                CreateRowCell(demos.Value2, Font10, tableDemos);
                CreateRowCell(demos.Validity, Font10, tableDemos);
            }

            return tableDemos;
        }

        public PdfPTable GetEndorserActivityTable(string ssn, string name, string endorserSsn, string endorserName, IEnumerable<Activity> orderedActivities)
        {
            PdfPTable tableActivity = new PdfPTable(5);
            tableActivity.SetWidths(new float[] { 1f, 1f, 2f, 4f, 1f });
            tableActivity.HeaderRows = 4;
            tableActivity.Summary = "Activity History";

            CreateHeaderCell("Activity History", FontLargeb, tableActivity, 5);
            tableActivity.CompleteRow();

            CreateHeaderCell($"Endorser: {endorserSsn} {endorserName}", Font10, tableActivity, 5, Element.ALIGN_LEFT);
            tableActivity.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableActivity, 5, Element.ALIGN_LEFT);
            tableActivity.CompleteRow();

            CreateHeaderCell("Date", Font10b, tableActivity);
            CreateHeaderCell("Time", Font10b, tableActivity);
            CreateHeaderCell("Activity Description", Font10b, tableActivity);
            CreateHeaderCell("Comment", Font10b, tableActivity);
            CreateHeaderCell("Recipient", Font10b, tableActivity);
            tableActivity.CompleteRow();

            foreach (var activity in orderedActivities)
            {
                CreateRowCell(activity.Date, Font10, tableActivity);
                CreateRowCell(activity.Time, Font10, tableActivity);
                CreateRowCell(activity.Description, Font10, tableActivity);
                CreateRowCell(activity.Comment, Font10, tableActivity);
                CreateRowCell(activity.Recipient, Font10, tableActivity);
            }

            return tableActivity;
        }

        public PdfPTable GetEndorserDefermentForbearanceTable(string ssn, string name, string endorserSsn, string endorserName, IEnumerable<Deferment> orderedDeferments)
        {
            PdfPTable tableDefermentForbearance = new PdfPTable(5);
            tableDefermentForbearance.SetWidths(new float[] { 1f, 1f, 1f, 1f, 2f });
            tableDefermentForbearance.HeaderRows = 4;
            tableDefermentForbearance.Summary = "Deferment/Forbearance History";

            CreateHeaderCell("Deferment/Forbearance History", FontLargeb, tableDefermentForbearance, 5);
            tableDefermentForbearance.CompleteRow();

            CreateHeaderCell($"Endorser: {endorserSsn} {endorserName}", Font10, tableDefermentForbearance, 5, Element.ALIGN_LEFT);
            tableDefermentForbearance.CompleteRow();

            CreateHeaderCell($"Borrower: {ssn} {name}", Font10, tableDefermentForbearance, 5, Element.ALIGN_LEFT);
            tableDefermentForbearance.CompleteRow();

            CreateHeaderCell("Loan Program", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Disbursement Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Start Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("End Date", Font10b, tableDefermentForbearance);
            CreateHeaderCell("Type", Font10b, tableDefermentForbearance);
            tableDefermentForbearance.CompleteRow();

            foreach (var deferment in orderedDeferments)
            {
                CreateRowCell(deferment.LoanProgram, Font10, tableDefermentForbearance);
                CreateRowCell(deferment.DisbursementDate, Font10, tableDefermentForbearance);
                CreateRowCell(deferment.Begin, Font10, tableDefermentForbearance);
                CreateRowCell(deferment.End, Font10, tableDefermentForbearance);
                CreateRowCell(deferment.Type, Font10, tableDefermentForbearance);
            }

            return tableDefermentForbearance;
        }
    }
}
