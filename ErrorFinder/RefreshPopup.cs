using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErrorFinder
{
    public partial class RefreshPopup : Form
    {
        public RefreshPopup(IEnumerable<BorrowerLine> lines)
        {
            InitializeComponent();
            this.lines = lines;
        }

        private void LoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null)
                t.Abort();
        }

        Thread t;
        IEnumerable<BorrowerLine> lines;
        private bool nelNet;
        private void RefreshPopup_Shown(object sender, EventArgs e)
        {
            t = new Thread(() =>
            {
                LogError("Begin");

                //ExecuteQuery("delete from Borrower_Errors;", "[Table Deletion Process]");
               // LogError("All table data deleted.");
                DataTable dt = new DataTable("Borrower_Errors_All");
                dt.Columns.Add("br_ssn");
                dt.Columns.Add("ln_seq");
                dt.Columns.Add("error_code");
                dt.Columns.Add("major_batch");
                dt.Columns.Add("minor_batch");
                dt.Columns.Add("AddedAt");
                DateTime d = DateTime.Now;
                foreach (BorrowerLine bl in lines)
                    dt.Rows.Add(bl.SSN, bl.SeqNo, bl.ErrorCode, bl.MajorBatch, bl.MinorBatchNo, d);
                LogError("Beginning Bulk Import");
                ExecuteBulkInsert(dt, "[Bulk Insert Process]");
                LogError("Bulk Import Complete");
                LogError("Refresh complete");
                this.Invoke(new Action(() =>
                {
                    RefreshProgress.Style = ProgressBarStyle.Blocks;
                    RefreshProgress.Maximum = 1;
                    RefreshProgress.Value = 1;
                }));
            });
            t.Start();
        }

        private void LogError(string error)
        {
            this.Invoke(new Action(() =>
            {
                ErrorList.Items.Insert(0, DateTime.Now.ToString("hh:mm:ss") + " - " + error);
            }));
        }

        private void ExecuteBulkInsert(DataTable dt, string label)
        {
            ExecuteViaConnection((conn) =>
            {
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.BatchSize = 10000;
                    bulk.DestinationTableName = dt.TableName;
                    bulk.BulkCopyTimeout = 60 * 45; //45 minutes
                    bulk.WriteToServer(dt);
                }
            }, label);
        }

        private void ExecuteQuery(string query, string label)
        {
            ExecuteViaConnection((conn) =>
            {
                using (SqlCommand comm = new SqlCommand(query, conn) { CommandTimeout = int.MaxValue })
                    try
                    {
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 701) //insufficient memory
                        {
                            LogError("Out of memory error for " + label + ".  Retrying.");
                            Thread.Sleep(1000);
                            ExecuteQuery(query, label);
                            return;
                        }
                        else
                            throw ex;
                    }
            }, label);
        }

        private void ExecuteViaConnection(Action<SqlConnection> action, string label)
        {
            string catalog = "EA27_BANA";
            using (SqlConnection conn = new SqlConnection("Data Source=opsdev;Initial Catalog=" + catalog + ";Integrated Security=SSPI;Connect Timeout=512000"))
            {
                try
                {
                    conn.Open();
                }
                catch (SqlException se)
                {
                    if (se.Number == 18452 || se.Number == 10054 || se.Number == 4060) //login failed (unknown reason)
                    {
                        LogError("Login failed for " + label + ".  Retrying.");
                        ExecuteViaConnection(action, label);
                        return;
                    }
                    else
                        throw se;
                }
                action(conn);
            }
        }
    }
}
