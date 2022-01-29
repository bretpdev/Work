using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Uheaa.Common.DataAccess
{
    public static partial class DataAccessHelper
    {
        public class ModeNotSetException : Exception { public ModeNotSetException(string message) : base(message) { } }
        public class RegionNotSetException : Exception { public RegionNotSetException(string message) : base(message) { } }
        #region Static Setup
        private static Region? currentRegion = null;
        public static bool RegionSet { get { return currentRegion.HasValue; } }
        public static Region CurrentRegion
        {
            get
            {
                if (!currentRegion.HasValue)
                    throw new RegionNotSetException("Data access region must be set before it can be used.");
                return currentRegion.Value;
            }
            set
            {
                currentRegion = value;
                EnterpriseFileSystem.Invalidate();
            }
        }
        private static Mode? currentMode = null;
        public static bool ModeSet { get { return currentMode.HasValue; } }
        public static Mode CurrentMode
        {
            get
            {
                if (!currentMode.HasValue)
                    throw new ModeNotSetException("Data access mode must be set before it can be used.");
                return currentMode.Value;
            }
            set
            {
                currentMode = value;
            }
        }
        public static bool TestMode
        {
            get
            {
                return ((CurrentMode == Mode.Test) || (currentMode == Mode.Dev) || (currentMode == Mode.QA));
            }
        }
        public enum Region
        {
            Uheaa,
            CornerStone,
            None,
            Pheaa

        }
        public enum Mode
        {
            Live,
            QA,
            Test,
            Dev,
            Local
        }

        //DO NOT change the number below
        public enum Database
        {
            Bsys = 0,
            Norad = 1,
            Csys = 2,
            Cdw = 3,
            Cls = 4,
            Udw = 5,
            Uls = 6,
            BatchProcessing = 7,
            MauiDude = 8,
            Reporting = 10,
            Income_Driven_Repayment = 11,
            SftpCoordinator = 99,
            ProcessLogs = 12,
            NeedHelpCornerStone = 14,
            NeedHelpUheaa = 15,
            IncidentReportingUheaa = 16,
            IncidentReportingCornerStone = 17,
            ECorrFed = 18,
            ImagingTransfers = 19,
            ACDC = 20,
            NelNetImport = 21,
            AlignImport = 22,
            Odw = 23,
            OperationsStatisticsCornerStone = 24,
            Scheduler = 25,
            IncomeBasedRepaymentUheaa = 26,
            ServicerInventoryMetrics = 27,
            DocId = 28,
            NobleCalls = 29,
            Income_Driven_RepaymentLegacy = 30,
            EmployeeHistory = 35,
            IVRControl = 36,
            Pls = 37,
            EcorrUheaa = 38,
            CentralData = 39,
            ReportServer = 40,
            Jams = 41,
            UheaaWebManagement = 42,
            AppDev = 43,
            Tlp = 44,
            Ols = 45,
            HumanResources = 46,
            CompleteFinancialFafsa = 47,
            UnexsysReports = 48,
            Voyager = 49,
            DynamicLetters = 50,
            RCProcessLogs = 51
        }
        private class DatabaseInfo
        {
            public static DatabaseInfo Standard(string dbName)
            {
                return Standard(dbName, "OPSDEV", "NOCHOUSETEST", "NOCHOUSEQA", "NOCHOUSE", @"(localdb)\MSSQLLocalDB");
            }
            public static DatabaseInfo StandardSecured(string dbName)
            {
                return Standard(dbName, "OPSDEV", "UHEAASQLDBTEST", "UHEAASQLDBQA", "UHEAASQLDB", @"(localdb)\MSSQLLocalDB");
            }
            public static DatabaseInfo OpsdevNochouse(string dbName)
            {
                return Standard(dbName, "opsdev", "opsdev", "opsdev", "NOCHOUSE", @"(localdb)\MSSQLLocalDB");
            }
            private static DatabaseInfo Standard(string dbName, string dev, string test, string qa, string live, string local)
            {
                return new DatabaseInfo("Data Source={0};Initial Catalog=" + dbName + ";Integrated Security=SSPI;",
                    Mode.Dev, dev,
                    Mode.Test, test,
                    Mode.QA, qa,
                    Mode.Live, live,
                    Mode.Local, local);
            }
            public Dictionary<Mode, string> Servers = new Dictionary<Mode, string>();
            public string ConnectionString { get; set; }
            public DatabaseInfo(string connectionString, params object[] servers)
            {
                ConnectionString = connectionString;
                for (int i = 0; i < servers.Length; i++)
                    Servers.Add((Mode)servers[i], (string)servers[++i]);
            }
            public DatabaseInfo(string connectionString, KeyValuePair<Mode, string>[] servers)
            {
                ConnectionString = connectionString;
                foreach (KeyValuePair<Mode, string> server in servers)
                    Servers.Add(server.Key, server.Value);
            }

            public string GetConnectionString(Mode mode)
            {
                return string.Format(ConnectionString, Servers[mode]);
            }

            public string GetConnectionString(Mode mode, string username, string password)
            {
                return string.Format(ConnectionString, Servers[mode], username, password);
            }
        }
        private static Dictionary<Database, DatabaseInfo> databases = new Dictionary<Database, DatabaseInfo>();
        static DataAccessHelper()
        {
            databases[Database.Bsys] = new DatabaseInfo("Data Source={0};Initial Catalog=BSYS;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "Nochouse", Mode.QA, "NochouseQA", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Norad] = new DatabaseInfo("Data Source={0};Initial Catalog=NORAD;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "Nochouse", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Csys] = new DatabaseInfo("Data Source={0};Initial Catalog=CSYS;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "Nochouse", Mode.QA, "NochouseQA", Mode.Test, "opsdev", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Cdw] = new DatabaseInfo("Data Source={0};Initial Catalog=CDW;Integrated Security=SSPI;MultipleActiveResultSets=True;Connection Timeout=100",
                Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Cls] = new DatabaseInfo("Data Source ={0}; Initial Catalog = CLS; integrated security = SSPI; persist security info = False; Trusted_Connection = Yes; MultipleActiveResultSets = True",
                Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "UHEAASQLDBTEST", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Udw] = new DatabaseInfo("Data Source={0};Initial Catalog=UDW;Integrated Security=SSPI;persist security info=False;Trusted_Connection=Yes;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.QA, "UHEAASQLDBTEST", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Uls] = new DatabaseInfo("Data Source={0};Initial Catalog=ULS;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Test, "UHEAASQLDBTEST", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.BatchProcessing] = new DatabaseInfo("Data Source={0};Initial Catalog=BatchProcessing;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.MauiDude] = new DatabaseInfo("workstation id=\"LPP-1494\";packet size=4096;integrated security=SSPI;data source={0};persist security info=False;initial catalog=MauiDUDE;MultipleActiveResultSets=True",
                Mode.Live, "Nochouse", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Reporting] = new DatabaseInfo("Data Source={0};Initial Catalog=Reporting;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "Nochouse", Mode.Dev, "OPSDEV", Mode.Test, "NochouseTest", Mode.QA, "NochouseQA", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Income_Driven_Repayment] = new DatabaseInfo("Data Source={0};Initial Catalog=Income_Driven_Repayment;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "UHEAASQLDBQA", Mode.Test, "UHEAASQLDBTEST", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ProcessLogs] = new DatabaseInfo("Data Source={0};Initial Catalog=ProcessLogs;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.NeedHelpCornerStone] = new DatabaseInfo("Data Source={0};Initial Catalog=NeedHelpCornerStone;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.Test, "NOCHOUSETEST", Mode.QA, "NOCHOUSEQA", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.NeedHelpUheaa] = new DatabaseInfo("Data Source={0};Initial Catalog=NeedHelpUheaa;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.Test, "NOCHOUSETEST", Mode.QA, "NOCHOUSEQA", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.IncidentReportingCornerStone] = new DatabaseInfo("Data Source={0};Initial Catalog=IncidentReportingCornerStone;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.Test, "NOCHOUSETEST", Mode.QA, "NOCHOUSEQA", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.IncidentReportingUheaa] = new DatabaseInfo("Data Source={0};Initial Catalog=IncidentReportingUheaa;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.Test, "NOCHOUSETEST", Mode.QA, "NOCHOUSEQA", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ECorrFed] = new DatabaseInfo("Data Source={0};Initial Catalog=ECorrFed;Integrated Security=SSPI;MultipleActiveResultSets=True",
               Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "UHEAASQLDBQA", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ImagingTransfers] = DatabaseInfo.OpsdevNochouse("ImagingTransfers");
            databases[Database.SftpCoordinator] = new DatabaseInfo("Data Source={0};Initial Catalog=SftpCoordinator;Integrated Security=SSPI;MultipleActiveResultSets=True",
               Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ACDC] = new DatabaseInfo("Data Source={0};Initial Catalog=ACDC;Integrated Security=SSPI;MultipleActiveResultSets=True",
               Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.QA, "NOCHOUSEAQ", Mode.Test, "NOCHOUSETEST", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.NelNetImport] = new DatabaseInfo("Data Source={0};Initial Catalog=NelNetImport;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "OPSDEV", Mode.Dev, @"(localdb)\MSSQLLocalDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.AlignImport] = new DatabaseInfo("Data Source={0};Initial Catalog=AlignImport;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, @"(localdb)\MSSQLLocalDB", Mode.Dev, @"(localdb)\MSSQLLocalDB", Mode.QA, @"(localdb)\MSSQLLocalDB", Mode.Test, @"(localdb)\MSSQLLocalDB", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Odw] = new DatabaseInfo("Data Source={0};Initial Catalog=ODW;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.OperationsStatisticsCornerStone] = new DatabaseInfo("Data Source={0};Initial Catalog=OperationsStatisticsCornerStone;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Scheduler] = new DatabaseInfo("Data Source={0};Initial Catalog=Scheduler;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.IncomeBasedRepaymentUheaa] = new DatabaseInfo("Data Source={0};Initial Catalog=IncomeBasedRepaymentUheaa;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "UHEAASQLDBTEST", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ServicerInventoryMetrics] = new DatabaseInfo("Data Source={0};Initial Catalog=ServicerInventoryMetrics;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.NobleCalls] = new DatabaseInfo("Data Source={0};Initial Catalog=NobleCalls;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.DocId] = new DatabaseInfo("Data Source={0};Initial Catalog=DocID;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "NOCHOUSE", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.EmployeeHistory] = new DatabaseInfo("Data Source={0};Initial Catalog=EmployeeHistory;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Income_Driven_RepaymentLegacy] = new DatabaseInfo("Data Source={0};Initial Catalog=Income_Driven_Repayment_Legacy;Integrated Security=SSPI;MultipleActiveResultSets=True",
               Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "UHEAASQLDBQA", Mode.Test, "UHEAASQLDBTEST", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.IVRControl] = new DatabaseInfo("Data Source={0};Initial Catalog=IVRControl;Integrated Security=SSPI;MultipleActiveResultSets=True",
               Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "UHEAASQLDBQA", Mode.Test, "UHEAASQLDBTEST", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Pls] = new DatabaseInfo("Data Source={0};Initial Catalog=PLS;Integrated Security=SSPI;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.EcorrUheaa] = new DatabaseInfo("Data Source={0};Initial Catalog=EcorrUheaa;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.CentralData] = new DatabaseInfo("Data Source={0};Initial Catalog=CentralData;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.ReportServer] = new DatabaseInfo("Data Source={0};Initial Catalog=ReportServer;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "UHEAASSRS", Mode.Dev, "UHEAASSRS", Mode.QA, "UHEAASSRS", Mode.Test, "UHEAASSRS", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Jams] = new DatabaseInfo("Data Source={0};Initial Catalog=JAMSV6;Integrated Security=SSPI;MultipleActiveResultSets=True",
              Mode.Live, "NOCHOUSE", Mode.Dev, "NOCHOUSE", Mode.QA, "NOCHOUSE", Mode.Test, "NOCHOUSE", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.UheaaWebManagement] = new DatabaseInfo("Data Source={0};Initial Catalog=UheaaWebManagement; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.AppDev] = new DatabaseInfo("Data Source={0};Initial Catalog=AppDev; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Dev, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Tlp] = new DatabaseInfo("Data Source={0};Initial Catalog=Tlp; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "NOCHOUSE", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Ols] = new DatabaseInfo("Data Source={0};Initial Catalog=OLS; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.UnexsysReports] = new DatabaseInfo("Data Source={0};Initial Catalog=UNEXSYSReports; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "UNexsysDialDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.HumanResources] = new DatabaseInfo("Data Source={0};Initial Catalog=HumanResources; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "USHESQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.CompleteFinancialFafsa] = new DatabaseInfo("Data Source={0};Initial Catalog=CompleteFinancialFafsa; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
                Mode.Live, "DBUHEAAOUTREACH", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.Voyager] = new DatabaseInfo("Data Source={0};Initial Catalog=voyager; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
               Mode.Live, "DBUHEAAOUTREACH", Mode.Dev, "DBUHOUTREACHDEV", Mode.QA, "DBUHOUTREACHDEV", Mode.Test, "DBUHOUTREACHDEV");
            databases[Database.DynamicLetters] = new DatabaseInfo("Data Source={0};Initial Catalog=DynamicLetters; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
               Mode.Live, "UHEAASQLDB", Mode.Dev, "OPSDEV", Mode.QA, "OPSDEV", Mode.Test, "OPSDEV", Mode.Local, @"(localdb)\MSSQLLocalDB");
            databases[Database.RCProcessLogs] = new DatabaseInfo("Data Source={0};Initial Catalog=ProcessLogs; integrated security=SSPI;persist security info=False;Trusted_Connection=Yes ;MultipleActiveResultSets=True",
               Mode.Live, "DBUHEAAOUTREACH", Mode.Dev, "DBUHOUTREACHDEV", Mode.QA, "DBUHOUTREACHDEV", Mode.Test, "DBUHOUTREACHDEV");
            MaxConnectionAgeInSeconds = 120;
            Console.SetOut(new ConsoleWriter());
        }
        #endregion

        public static void UseDefaultConsoleWriter()
        {
            ConsoleWriter.UseDefaultWriter = true;
        }

        public static void UseCustomConsoleWriter()
        {
            ConsoleWriter.UseDefaultWriter = false;
        }

        public static string GetConnectionString(Database db)
        {
            return GetConnectionString(db, CurrentMode);
        }
        public static string GetConnectionString(Database db, Mode mode)
        {
            return databases[db].GetConnectionString(mode);
        }
        /// <summary>
        /// Gets a datacontext with the current mode and given database.
        /// </summary>
        public static DataContext GetContext(Database db)
        {
            return GetContext(db, CurrentMode);
        }
        /// <summary>
        /// Gets a datacontext with the specified mode and database.
        /// </summary>
        private static DataContext GetContext(Database db, Mode mode)
        {
            return new DataContext(GetConnectionString(db, mode));
        }
        /// <summary>
        /// Gets a datacontext with the current mode and given database using the specified username and password.
        /// </summary>
        private static DataContext GetContext(Database db, string username, string password)
        {
            return new DataContext(databases[db].GetConnectionString(CurrentMode, username, password));
        }


        /// <summary>
        /// Returns a command with an open connection to the given database.
        /// </summary>
        public static SqlCommand GetCommand(string commandName, Database db, Mode mode)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString(db, mode));
            conn.Open();
            SqlCommand comm = new SqlCommand(commandName, conn);
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            return comm;
        }

        /// <summary>
        /// Returns a SqlDataAdapter.
        /// </summary>
        public static SqlDataAdapter GetAdapter(string commandName, Database db, Mode mode)
        {
            return GetAdapter(GetCommand(commandName, db, mode));
        }
        /// <summary>
        /// Returns a SqlDataAdapter.
        /// </summary>
        public static SqlDataAdapter GetAdapter(SqlCommand comm)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(comm);
            return adapter;
        }

        /// <summary>
        /// Executes the given command and returns a single int.  Your stored procedure should select SCOPE_IDENTITY()
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteId(string commandName, Database db, params SqlParameter[] parameters)
        {
            //SCOPE_IDENTITY() returns a decimal, so we convert to string and them back to int
            //to ensure we get a normal primary key value
            object id = ExecuteSingle<object>(commandName, db, parameters);
            return id.ToString().ToInt();
        }

        /// <summary>
        /// Executes the given command and returns a single int.  Your stored procedure should select SCOPE_IDENTITY()
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int? ExecuteIdNullable(string commandName, Database db, params SqlParameter[] parameters)
        {
            //SCOPE_IDENTITY() returns a decimal, so we convert to string and them back to int
            //to ensure we get a normal primary key value
            object id = ExecuteSingle<object>(commandName, db, parameters);
            return (id ?? "").ToString().ToIntNullable();
        }

        /// <summary>
        /// Executes the given command and returns a single int.  Your stored procedure should select SCOPE_IDENTITY()
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteId(string commandName, SqlConnection conn, params SqlParameter[] parameters)
        {
            //SCOPE_IDENTITY() returns a decimal, so we convert to string and them back to int
            //to ensure we get a normal primary key value
            object id = ExecuteSingle<object>(commandName, conn, parameters);
            return id.ToString().ToInt();
        }

        public static T ExecuteSingle<T>(string commandName, SqlConnection conn, params SqlParameter[] parameters)
        {
            List<T> results = ExecuteList<T>(commandName, conn, parameters);
            if (results.Count == 0)
                throw new InvalidOperationException(string.Format("Command: {0} returned no results.", commandName));
            return results[0];
        }

        /// <summary>
        /// Executes the given command and returns a single result.
        /// </summary>
        public static T ExecuteSingle<T>(string commandName, Database db, params SqlParameter[] parameters)
        {
            return ExecuteSingle<T>(commandName, db, CurrentMode, parameters);
        }
        /// <summary>
        /// Executes the given command and returns a single result.
        /// </summary>
        public static T ExecuteSingle<T>(string commandName, Database db, Mode mode, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = GetCommand(commandName, db, mode))
            {
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
                return ExecuteSingle<T>(comm);
            }
        }
        /// <summary>
        /// Executes the given command and returns a single result.
        /// </summary>
        private static T ExecuteSingle<T>(SqlCommand comm)
        {
            List<T> results = ExecuteList<T>(comm);
            if (results.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Command: {0} returned no results.", comm.CommandText));
            }
            return results[0];
        }

        public static List<T> ExecuteList<T>(string commandName, SqlConnection conn, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = new SqlCommand(commandName, conn))
            {
                try
                {

                    comm.CommandTimeout = int.MaxValue;
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (SqlParameter param in parameters)
                        comm.Parameters.Add(param);
                    List<T> results = new List<T>();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        try
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    results.Add(Populate<T>(reader)); //primitive type
                                }
                            } while (reader.NextResult());
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                        }
                    }
                    return results;
                }
                finally
                {
                    comm.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Executes the given command and returns a list of the results.
        /// </summary>
        private static List<T> ExecuteList<T>(string commandName, Database db)
        {
            return ExecuteList<T>(GetCommand(commandName, db, CurrentMode));
        }

        /// <summary>
        /// Executes the given command in the given amount of time and returns a list of the results.
        /// </summary>
        /// <returns></returns>
        public static List<T> ExecuteList<T>(string commandName, Database db, int connectionTimeout, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = GetCommand(commandName, db, CurrentMode))
            {
                comm.CommandTimeout = connectionTimeout;
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
                return ExecuteList<T>(comm);
            }
        }

        /// <summary>
        /// Executes the given command and returns a list of the results. 
        /// </summary>
        public static List<T> ExecuteList<T>(string commandName, Database db, params SqlParameter[] parameters)
        {
            return ExecuteList<T>(commandName, db, CurrentMode, parameters);
        }

        /// <summary>
        /// Executes the given command and returns a list of the results. 
        /// </summary>
        public static List<T> ExecuteList<T>(string commandName, Database db, Mode mode, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = GetCommand(commandName, db, mode))
            {
                comm.CommandTimeout = int.MaxValue;
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
                return ExecuteList<T>(comm);
            }
        }
        /// <summary>
        /// Executes the given command and returns a list of the results.  Only works on Stored Procedures that select only one field.
        /// </summary>
        private static List<T> ExecuteList<T>(SqlCommand comm)
        {
            try
            {
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandTimeout = int.MaxValue;
                List<T> results = new List<T>();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    do
                    {
                        while (reader.Read())
                        {
                            results.Add(Populate<T>(reader)); //primitive type
                        }
                    } while (reader.NextResult());
                }
                comm.Connection.Close();
                return results;
            }
            finally
            {
                comm.Parameters.Clear();
            }
        }

        public static DataTable ExecuteDataTable(string query, SqlConnection conn, bool throwEx = true, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = new SqlCommand(query, conn))
            {
                comm.CommandTimeout = int.MaxValue;
                comm.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);


                var t = ExecuteDataSet(comm);

                foreach (DataTable table in t.Tables)
                {
                    if (table.Rows.Count != 0)
                        return table;
                    else if (!throwEx)
                        return table;
                }

                string exceptionMessage = string.Format("No results returned for sproc {0}, SqlParameters {1}", query,
                    string.Join(";", parameters.Select(p => new { val = string.Format("{0}:{1}", p.ParameterName, p.Value) }).ToList()));
                throw new Exception(exceptionMessage);
            }
        }

        public static DataTable ExecuteDataTable(string query, Database database, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString(database)))
            {
                return ExecuteDataTable(query, conn, true, parameters);
            }
        }

        public static DataTable ExecuteDataTable(string query, Database database, bool throwEx = true, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString(database)))
            {
                return ExecuteDataTable(query, conn, throwEx, parameters);
            }
        }

        /// <summary>
        /// Executes the given command and returns the results as a data table.
        /// </summary>
        public static DataTable ExecuteDataTable(string query, Database database)
        {
            return ExecuteDataTable(query, database, CurrentMode);
        }
        /// <summary>
        /// Executes the given command and returns the results as a data table.
        /// </summary>
        public static DataTable ExecuteDataTable(string query, Database database, Mode mode)
        {
            using (SqlCommand comm = GetCommand(query, database, mode))
            {
                return ExecuteDataSet(comm).Tables[0];
            }
        }

        /// <summary>
        /// Executes the given command and returns the results as a data set.
        /// </summary>
        private static DataSet ExecuteDataSet(SqlCommand comm)
        {
            DataSet ds = new DataSet();
            comm.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter adapter = GetAdapter(comm))
                adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Execute the given stored procedure and return nothing.
        /// </summary>
        public static void Execute(string commandName, Database db, params SqlParameter[] parameters)
        {
            Execute(commandName, db, CurrentMode, parameters);
        }

        /// <summary>
        /// Execute the given stored procedure and return nothing. 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="db"></param>
        /// <param name="connectionTimeout">Timeout value in seconds</param>
        /// <param name="parameters"></param>
        public static void Execute(string commandName, Database db, int connectionTimeout, params SqlParameter[] parameters)
        {
            Execute(commandName, db, CurrentMode, connectionTimeout, parameters);
        }

        public static void Execute(string commandName, SqlConnection conn, params SqlParameter[] parameters)
        {
            Execute(commandName, conn, 90, parameters);
        }

        public static void Execute(string commandName, SqlConnection conn, int connectionTimeOut, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = new SqlCommand(commandName, conn))
            {
                comm.CommandTimeout = connectionTimeOut;
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);


                Execute(comm);
            }
        }

        /// <summary>
        /// Execute the given stored procedure and return nothing.
        /// </summary>
        public static void Execute(string commandName, Database db, Mode mode, params SqlParameter[] parameters)
        {
            Execute(commandName, db, mode, 90, parameters);
        }

        /// <summary>
        /// Execute the given stored procedure and return nothing.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="db"></param>
        /// <param name="mode"></param>
        /// <param name="connectionTimeout">Timeout value in seconds</param>
        /// <param name="parameters"></param>
        public static void Execute(string commandName, Database db, Mode mode, int connectionTimeout, params SqlParameter[] parameters)
        {
            using (SqlCommand comm = GetCommand(commandName, db, mode))
            {
                comm.CommandTimeout = connectionTimeout; //This is done in seconds, not milliseconds
                comm.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
                Execute(comm);
                comm.Parameters.Clear();
                comm.Connection.Close();
                comm.Connection.Dispose();
            }
        }

        /// <summary>
        /// Execute the given SqlCommand and return nothing.
        /// </summary>
        /// <param name="comm">The SqlCommand to execute.</param>
        private static void Execute(SqlCommand comm)
        {
            comm.ExecuteNonQuery();
        }

        #region Population
        public class PopulationException : Exception
        {
            public PopulationException(string message, Exception innerException)
                : base(message, innerException)
            { }
        }

        /// <summary>
        /// Creates and populates an object of type T using data from a SqlDataReader
        /// </summary>
        /// <typeparam name="T">The type of object to create (primitives or classes)</typeparam>
        /// <param name="reader">The reader to pull columns from.  This should already be set to a row.</param>
        /// <param name="read">True if the .Read() method on the reader should be called.</param>
        /// <returns>A populated object of type T</returns>
        public static T Populate<T>(DbDataReader reader, bool read = false)
        {
            if (read)
                reader.Read();
            if (!reader.HasRows)
                return default(T);
            Type type = typeof(T);
            object value = reader[0] is DBNull ? null : reader[0];
            if (type.GetConstructor(Type.EmptyTypes) == null) //no default constructor
                return (T)value; //primitive types
            if (typeof(T) == typeof(object)) //they just want a base object type, return it
                return (T)value;
            List<string> columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i).ToLower());
            T obj = (T)type.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            foreach (PropertyInfo pi in type.GetProperties())
            {
                string dbName = pi.GetAttributeValue<DbNameAttribute, string>(o => o.Name, pi.Name);
                dbName = dbName.ToLower();
                if (columns.Contains(dbName))
                    try
                    {
                        pi.SetValue(obj, ConvertCustom(reader[dbName], pi.PropertyType), new object[] { });
                    }
                    catch (Exception ex)
                    {
                        throw new PopulationException(string.Format("Couldn't populate field {0} of class {1}.  The value was {2}.  \nOriginal Exception: \n{3}", pi.Name, obj.ToString(), reader[dbName], ex.Message), ex);
                    }
            }
            return obj;
        }

        /// <summary>
        /// Convert specified value to a given type.  Special string parsing includes:
        /// Money with dollar signs converted to decimal ("$5.23" -> 5.23m)
        /// Y/N converted to bool ("y" -> true)
        /// 1/0 converted to bool ("0" -> false)
        /// String dates parsed to dates ("1/1/2015" -> new Date(1, 1, 2015))
        /// </summary>
        private static object ConvertCustom(object value, Type type)
        {
            if (value is DBNull)
                if (type == typeof(DateTime))
                    throw new Exception("Can't convert DBNull to non-nullable DateTime");
                else
                    return null;
            if (Nullable.GetUnderlyingType(type) != null)//working with a nullable)
            {
                if (value is string)
                    if (string.IsNullOrEmpty((string)value))
                        return null;
                type = Nullable.GetUnderlyingType(type);
            }
            if (value is int && type.IsEnum)
                return Enum.ToObject(type, value);
            if (type == typeof(bool))
                if (value is string)
                {
                    //returns 0/1 and Y/N as bools
                    string sval = (string)value;
                    sval = sval.ToLower();
                    if (sval == "y" || sval == "yes" || sval == "1") return true;
                    if (sval == "n" || sval == "no" || sval == "0") return false;
                }
            if (type == typeof(DateTime))
                if (value is string)
                {
                    DateTime parse = DateTime.Now;
                    if (DateTime.TryParse((string)value, out parse))
                        return parse;
                }
            if (type == typeof(decimal))
                if (value is string)
                {
                    string sval = (string)value;
                    sval = sval.Replace("$", "");
                    decimal parse = 0;
                    if (decimal.TryParse(sval, out parse))
                        return parse;
                }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Will take an object reflect into is properities and generate a list of sql params
        /// </summary>
        /// <param name="obj">Object to create the SqlParam list out of</param>
        /// <returns>A list of SqlParameters</returns>
        public static List<SqlParameter> GenerateSqlParamsFromObject(object obj)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            foreach (PropertyInfo item in obj.GetType().GetProperties())
                sqlParams.Add(new SqlParameter(item.Name, obj.GetType().GetProperty(item.Name).GetValue(obj, null)));

            return sqlParams;
        }
        #endregion

        public static bool StandardArgsCheck(string[] args, string applicationTitle)
        {
            return StandardArgsCheck(args, applicationTitle, true);
        }

        /// <summary>
        /// Sets the CurrentMode based on the given args.  If the args are invalid, warns the user via Message Box.
        /// </summary>
        /// <returns>True if successful, False otherwise.</returns>
        public static bool StandardArgsCheck(string[] args, string applicationTitle, bool showMessage)
        {
            switch (args.Count() > 0 ? args.FirstOrDefault().ToLower() : "")
            {
                case "dev":
                    CurrentMode = Mode.Dev;
                    return true;
                case "test":
                    CurrentMode = Mode.Test;
                    return true;
                case "qa":
                    CurrentMode = Mode.QA;
                    return true;
                case "live":
                    CurrentMode = Mode.Live;
                    return true;
                default:
                    if (showMessage)
                        new CreateShortcutForm(applicationTitle).ShowDialog();
                    else
                        Console.WriteLine("Please start this application with a first argument of either dev, test, qa, or live.");
                    return false;
            }
        }

        /// <summary>
        /// Checks the stored procedures to make sure they all exist and the user has access to them
        /// </summary>
        public static bool CheckSprocAccess(Assembly assembly, bool showMessage = true)
        {
            string errorMessage = "The following stored procedures are either missing or you do not have access to run them.\r\n\r\n{0}\r\nPlease contact system support to get access.";
            List<string> errors = new List<string>();
            List<SprocValidationResult> sprocs = DatabaseAccessHelper.CheckSprocAccess(assembly);
            foreach (SprocValidationResult result in sprocs)
            {
                if (result.Result == SprocResult.Failure || result.Result == SprocResult.Error)
                    errors.Add(result.Attribute.SprocName);
            };

            if (errors.Count() > 0)
            {
                string error = "";
                foreach (string item in errors)
                {
                    error += "- " + item + "\r\n";
                }
                string errorWMessage = string.Format(errorMessage, error);
                if (showMessage)
                    Dialog.Error.Ok(errorWMessage);
                else
                    Console.WriteLine(errorWMessage);

                return false;
            }
            return true;
        }

        public static List<T> ParseDataTable<T>(DataTable dt)
        {
            List<T> results = new List<T>();
            using (DataTableReader reader = dt.CreateDataReader())
                while (reader.Read())
                    results.Add(Populate<T>(reader));
            return results;
        }

        /// <summary>
        /// Takes in a IEnumerable<object> and returns a DataTable with the data set
        /// </summary>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            DataTable dt = new DataTable();

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}