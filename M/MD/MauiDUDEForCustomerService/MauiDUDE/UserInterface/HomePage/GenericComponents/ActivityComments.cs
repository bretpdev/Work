using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using System.Diagnostics.PerformanceData;

namespace MauiDUDE
{
    public partial class ActivityComments : UserControl
    {
        public enum AESSystem
        {
            Compass,
            OneLINK
        }

        private ActivityCommentGatherer Processor { get; set; }
        
        /// <summary>
        /// Tracks of comments were successfully collected
        /// </summary>
        public bool Success { get; set; }


        public ActivityComments()
        {
            InitializeComponent();
        }

        public void PopulateComments(ActivityCommentGatherer.DaysOrNumberOf daysOrNumberOf, int count, bool userRequested, string personID, AESSystem system, DataAccessHelper.Region region)
        {
            Processor = new ActivityCommentGatherer(personID); //create processor
            if(system == AESSystem.OneLINK)
            {
                Success = SetupLP50History(daysOrNumberOf, count);
            }
            else
            {
                Success = PopulateActivityHistoryDisplayFromTD2A(daysOrNumberOf, count, region);
            }
            if(!Success && userRequested)
            {
                WhoaDUDE.ShowWhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE");
            }
        }

        public void PopulateCommentsFromWarehouse(string accountNumber, string recipientId, DataAccessHelper.Region region, bool userRequested)
        {
            //This is the table that will fill the datagrid
            DataTable activityHistoryTable = new DataTable();
            activityHistoryTable.Columns.Add("Request Code");
            activityHistoryTable.Columns.Add("Response Code");
            activityHistoryTable.Columns.Add("Request Description");
            activityHistoryTable.Columns.Add("Request Date");
            activityHistoryTable.Columns.Add("Response Date");
            activityHistoryTable.Columns.Add("Requestor");
            activityHistoryTable.Columns.Add("Performed");
            activityHistoryTable.Columns.Add("Text");

            List<ActivityHistoryRecord> activityHistory = new List<ActivityHistoryRecord>();
            activityHistory = DataAccess.DA.GetReferenceActivityHistory(accountNumber, recipientId, region);

            foreach(var row in activityHistory)
            {
                BindingList<string> columnVals = new BindingList<string>();
                columnVals.Add(row.RequestCode);
                columnVals.Add(row.ResponseCode);
                columnVals.Add(row.RequestDescription);
                columnVals.Add(row.RequestDate);
                columnVals.Add(row.ResponseDate);
                columnVals.Add(row.Requestor);
                columnVals.Add(row.PerformedDate);
                columnVals.Add(row.CommentText);
                activityHistoryTable.Rows.Add(columnVals);
            }

            if(activityHistoryTable.Rows.Count == 0)
            {
                WhoaDUDE.ShowWhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE");
            }
            else
            {
                dataGridView.DataSource = new DataView(activityHistoryTable);
                ApplyGridFormatting();
            }
        }

        private bool PopulateActivityHistoryDisplayFromTD2A(ActivityCommentGatherer.DaysOrNumberOf daysOrNumberOf, int count, DataAccessHelper.Region region)
        {
            DataTable activityHistoryTable = Processor.PopulateActivityHistoryFromTD2A(daysOrNumberOf,count,region);

            if(activityHistoryTable.Rows.Count == 0)
            {
                //no history
                return false;
            }
            else
            {
                dataGridView.DataSource = new DataView(activityHistoryTable);
                ApplyGridFormatting();
                return true;
            }
        }

        private bool SetupLP50History(ActivityCommentGatherer.DaysOrNumberOf daysOrNumberof, int count)
        {
            DataTable activityHistoryTable = Processor.PopulateActivityHistoryFromLP50(daysOrNumberof, count);

            if(activityHistoryTable.Rows.Count == 0)
            {
                //no history
                return false;
            }
            else
            {
                dataGridView.DataSource = new DataView(activityHistoryTable);
                ApplyGridFormatting();
                return true;
            }
        }

        private void ApplyGridFormatting()
        {
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            //dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dataGridView.AutoResizeColumns();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.Columns["Text"].Width = 289;
            //dataGridView.Columns["Text"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.Columns["Request Description"].Width = 150;
            //dataGridView.Columns["Request Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.Columns["Request Code"].Width = 90;
            dataGridView.Columns["Response Code"].Width = 90;
            dataGridView.Columns["Request Date"].Width = 90;
            dataGridView.Columns["Response Date"].Width = 90;
            dataGridView.Columns["Requestor"].Width = 90;
            dataGridView.Columns["Performed"].Width = 90;

            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AutoResizeRows();
        }
    }
}
