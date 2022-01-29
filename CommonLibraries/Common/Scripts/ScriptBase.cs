using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.Scripts
{

    public interface IHasMain
    {
        void Main();
    }

    public partial class ScriptBase : IHasMain
    {
        private ReflectionInterface ri;
        private string scriptId;
        private string userId;

        public enum LoanStatus
        {
            Closed,
            Open,
            None
        }

        public enum AesSystem
        {
            None = 0,
            Compass = 1,
            Lco = 2,
            OneLink = 4,
            CompassAndOneLink = 5
        }

        protected ReflectionInterface RI { get { return ri; } }
        protected string ScriptId { get { return scriptId; } }
        public string UserId { get { return userId; } }
        protected virtual RecoveryLog Recovery { get; set; }
        public ProcessLogData ProcessLogData { get; set; }
        public bool CalledByMauiDUDE { get; set; } = false;
        public MDBorrower MauiDUDEBorrower { get; set; }

        public ScriptBase(ReflectionInterface ri, string scriptId, DataAccessHelper.Region region)
            : this(ri, scriptId)
        {
            DataAccessHelper.CurrentRegion = region;
            ValidateRegion(region);
			ProcessLogData = ri.ProcessLogData ?? ProcessLogger.ProcessLogger.RegisterScript(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// MauiDUDE constructor for ScriptBase
        /// DO NOT USE IF YOU ARE NOT TAKING AN MDBorrower from MD
        /// </summary>
        public ScriptBase(ReflectionInterface ri, string scriptId, DataAccessHelper.Region region, MDBorrower borrower)
            : this(ri, scriptId, borrower)
        {
            DataAccessHelper.CurrentRegion = region;
            ValidateRegion(region);
            ProcessLogData = ri.ProcessLogData ?? ProcessLogger.ProcessLogger.RegisterScript(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
        }

        public ScriptBase()
        {
            //use if we do not want to use scriptbase but scriptco makes us
        }

        public ScriptBase(ReflectionInterface ri, string scriptId)
        {
            this.ri = ri;
            this.scriptId = scriptId;
            this.userId = GetUserID();
            try
            {
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.None)
                    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            }
            catch (DataAccessHelper.RegionNotSetException)
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            }
            ProcessLogData = ri.ProcessLogData ?? ProcessLogger.ProcessLogger.RegisterScript(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// MauiDUDE constructor for ScriptBase
        /// DO NOT USE IF YOU ARE NOT TAKING AN MDBorrower from MD
        /// </summary>
        public ScriptBase(ReflectionInterface ri, string scriptId, MDBorrower borrower)
        {
            CalledByMauiDUDE = true;
            MauiDUDEBorrower = borrower;
            this.ri = ri;
            this.scriptId = scriptId;
            this.userId = GetUserID();
            try
            {
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.None)
                    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            }
            catch (DataAccessHelper.RegionNotSetException)
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            }
            ProcessLogData = ri.ProcessLogData ?? ProcessLogger.ProcessLogger.RegisterScript(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
        }

        public virtual void Main() { }

        /// <summary>
        /// Throws EndDLLException to end DLL functionality (this should be caught and handled by your main method)
        /// </summary>
        protected void EndDllScript()
        {
            if (ProcessLogData != null && ProcessLogData.ProcessLogId > 0)
                ProcessLogger.ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);

            throw new EndDLLException("This exception is thrown to allow the script to exit gracefully.  Please contact a member of Systems Support if you receive it.");
        }

        protected void ValidateRegion(DataAccessHelper.Region region)
        {
            if (!RI.ValidateRegion(region))
                EndDllScript();
        }
        /// <summary>
        /// Gets demographic information from screen TX1J.
        /// </summary>
        /// <param name="ssnOrAcctNum">The SSN or Account Number.</param>
        /// <returns>A populated SystemBorrowerDemograhics object.</returns>
        protected SystemBorrowerDemographics GetDemographicsFromTx1j(string ssnOrAcctNum)
        {
            return RI.GetDemographicsFromTx1j(ssnOrAcctNum);
        }

        /// <summary>
        /// Goes to the PROF screen
        /// </summary>
        /// <returns>returns a user id from location 2, 49</returns>
        private string GetUserID()
        {
            RI.FastPath("PROF");
            return RI.GetText(2, 49, 7);
        }

        /// <summary>
        /// Receives a comma delimited file with a header row and parses it into a Data Table
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Data Table with the data from the file passed in.</returns>
        public DataTable CreateDataTableFromFile(string fileName)
        {
            return FileSystemHelper.CreateDataTableFromFile(fileName);
        }

        public LoanStatus GetCompassLoanStatus(string ssn)
        {
            List<string> closedStatus = new List<string>() { "DECONVERTED", "PAID IN FULL", "CLAIM PAID" };
            LoanStatus status = LoanStatus.None;

            if (ssn.StartsWith("P"))
            {
                RI.FastPath("TX3ZITX1J;" + ssn);
                ssn = RI.GetText(7, 11, 11).Replace(" ", "");
            }
            else if (ssn.StartsWith("RF@"))
            {
                RI.FastPath("LP2CI;" + ssn);
                ssn = RI.GetText(3, 39, 9);
            }
            bool foundOpenLoan = false;
            bool foundClosedLoan = false;
            RI.FastPath("TX3ZITS26" + ssn);
            if (RI.CheckForText(1, 72, "TSX29")) //Target Screen
            {
                if (RI.CheckForText(3, 10, closedStatus.ToArray()))
                    status = LoanStatus.Closed;
                else
                    status = LoanStatus.Open;
            }
            else if (RI.CheckForText(1, 72, "TSX28"))
            {
                int row = 8;
                while (!RI.CheckForText(row, 2, "  ") && !RI.CheckForText(23, 2, "90007"))
                {
                    RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true); //Select the loan
                    if (RI.CheckForText(3, 10, closedStatus.ToArray()))
                        status = LoanStatus.Closed;
                    else
                        status = LoanStatus.Open;
                    foundOpenLoan = status == LoanStatus.Open ? true : false;
                    foundClosedLoan = status == LoanStatus.Closed ? true : false;
                    if (foundOpenLoan)
                        break;
                    RI.Hit(ReflectionInterface.Key.F12);
                    row++;
                    if (row > 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }
                }
            }
            return status;
        }
    }
}
