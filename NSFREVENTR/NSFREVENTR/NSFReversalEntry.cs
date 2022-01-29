using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace NSFREVENTR
{
    public class NSFReversalEntry : ScriptBase
    {

        public enum System
        {
            OneLINK,
            Compass,
            None
        }

        public enum BatchType
        {
            Cash,
            Wire,
            None
        }

        public enum LoanListLocation
        {
            All,
            SeeListBelow
        }

        //Consts
        public const string COMPASS_NSF_ENTRY_FILE = "NSF ENTRY.txt";
        public const string COMPASS_NSF_NON_POSTING_ENTRY_FILE = "NON POSTING NSF ENTRY.txt";
        public const string COMPASS_DIR = @"X:\PADD\OPERATIONAL ACCOUNTING\COMPASS\";
        public const string ONELINK_DIR = @"X:\PADD\OPERATIONAL ACCOUNTING\OneLINK\";
        public const string ONELINK_NSF_ENTRY_FILE = "NSF ENTRY.txt";
        public TestModeResults TMR { get; set; }

        public ReversalEntry RevEntry { get; set; }

        public NSFReversalEntry(ReflectionInterface ri)
            : base(ri, "NSFREVENTR")
        {
            try
            {
                ValidateRegion(Region.UHEAA);
            }
            catch (StupRegionSpecifiedException ex)
            {
                MessageBox.Show("You must be in the UHEAA region to run this script.");
                Environment.Exit(1);
            }
        }

        public override void Main()
        {
            TMR = TestMode(COMPASS_DIR);
            EntryForm entryFrm = new EntryForm(System.Compass, TestModeProperty);
            while (true) //go until the user stops
            {
                DialogResult result = entryFrm.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    EndDLLScript();
                }
                else if (result == DialogResult.OK)
                {
                    //process new entry
                    RevEntry = entryFrm.UserProvidedEntry;
                    if (RevEntry.SSN.Length == 10) RevEntry.SSN = GetDemographicsFromTX1J(RevEntry.SSN).SSN;
                    BorrowerDemographics demos = GetDemographicsFromTX1J(RevEntry.SSN);
                    try
                    {
                        CompassProcessor processor;
                        //if (RevEntry.System == System.Compass)
                        //{
                        //do account number conversion to SSN if account number is provided
                        processor = new CompassProcessor(RevEntry, RI, GetUserIDFromLP40(), this, demos);
                        //}
                        //else //OneLINK
                        //{
                        //    processor = new OneLINKProcessor(RevEntry, RI);
                        //}
                        processor.ProcessEntry(); //process entry
                    }
                    catch (ExitScriptException ex)
                    {
                        MessageBox.Show(ex.Message, "Error Encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        EndDLLScript();
                    }
                    MessageBox.Show("Processing Complete!", "Processing Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.Yes) //print button clicked
                {
                    //print batch reports
                    RevEntry = entryFrm.UserProvidedEntry;
                    try
                    {
                        BatchReportPrinting printer;
                        if (RevEntry.System == System.Compass)
                        {
                            //print reports
                            printer = new CompassBatchReportPrinting(TestModeProperty);
                        }
                        else //OneLINK
                        {
                            //print reports
                            printer = new OneLINKBatchReportPrinting(TestModeProperty);
                        }
                        printer.GenerateAndPrintReports();
                    }
                    catch (ExitScriptException ex)
                    {
                        MessageBox.Show(ex.Message, "Error Encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        EndDLLScript();
                    }
                    MessageBox.Show("Please retrieve your documents from the printer.", "Printing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                entryFrm = new EntryForm(RevEntry.System, TestModeProperty);
            }
        }

        public void EndScript()
        {
            EndDLLScript();
        }

    }
}
