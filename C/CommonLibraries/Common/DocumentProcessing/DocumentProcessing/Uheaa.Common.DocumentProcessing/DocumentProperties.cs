using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.DocumentProcessing
{
    public class DocumentProperties
    {
        public enum CorrMethod
        {
            Printed,
            EmailNotify,
            DirectDebit
        }
        //Note these properties are not following oue naming convention as they will be used to generate a xml file 
        //for the E-Corr process.  We will be using reflection and use the properties names.
        public int DocumentDetailsId { get; set; }
        public string PATH { get; set; }
        public string SSN { get; set; }
        public DateTime DOC_DATE { get; set; }
        public string DOC_ID { get; set; }
        public string ADDR_ACCT_NUM { get; set; }
        public string LETTER_ID { get; set; }
        public string REQUEST_USER { get; set; }
        public string VIEWABLE { get; set; }
        public string CORR_METHOD { get; set; }
        public string REPORT_DESC { get; set; }
        public DateTime LOAD_TIME { get; set; }
        public string REPORT_NAME { get; set; }
        public string ADDRESSEE_EMAIL { get; set; }
        public string VIEWED { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? DUE_DATE { get; set; }
        public string MAINFRAME_REGION { get; set; }
        private string dcn;
        public string DCN
        {
            get
            {
                return dcn.PadLeft(17, '0');
            }
            set
            {
                dcn = value;
            }
        }
        public string TOTAL_DUE { get; set; }
        public string BILL_SEQ { get; set; }
        public string SUBJECT_LINE { get; set; }
        public string DOC_SOURCE { get; set; }
        public string DOC_COMMENT { get; set; }
        public string WORKFLOW { get; set; }
        public string DOC_DELETE { get; set; }
        public DataAccessHelper.Region Region { get; set; }
        public int CorrespondenceFormatId { get; set; }

        [Obsolete("This method should only be used by DataAccessHelper", true)]
        public DocumentProperties() { }

        private string TranslateCorrMethod(CorrMethod ecorrMethod, bool isAltFormat)
        {
            if (isAltFormat)
                return "Printed";

            switch (ecorrMethod)
            {
                case CorrMethod.Printed:
                    return "Printed";
                case CorrMethod.EmailNotify:
                    return "Email Notify";
                case CorrMethod.DirectDebit:
                    return "Direct Debit";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Only Use For UHEAA
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="addrAcctNum"></param>
        /// <param name="letterId"></param>
        /// <param name="requestorId"></param>
        /// <param name="email"></param>
        /// <param name="ecorrMethod"></param>
        /// <param name="fileName"></param>
        public DocumentProperties(string ssn, string addrAcctNum, string letterId, string requestorId, string email, CorrMethod ecorrMethod, string fileName, DateTime? duedate = null, decimal? totalDue = null, int? billSeq = null, DateTime? createDate = null)
        {
            DateTime currentDate = DateTime.Now;
            ADDR_ACCT_NUM = addrAcctNum;
            SSN = ssn;
            DOC_DATE = currentDate;
            LETTER_ID = letterId;
            REQUEST_USER = requestorId;
            LOAD_TIME = billSeq.HasValue ? createDate.Value : currentDate;
            ADDRESSEE_EMAIL = email;
            CREATE_DATE = createDate.HasValue ? createDate.Value : currentDate;
            CORR_METHOD = ecorrMethod.ToString();
            PATH = fileName;
            DUE_DATE = duedate;
            TOTAL_DUE = totalDue.HasValue ? totalDue.ToString() : null;
            BILL_SEQ = billSeq.HasValue ? billSeq.ToString() : null;
        }

        /// <summary>
        /// Letters Constructor
        /// </summary>
        public DocumentProperties(string ssn, string addrAcctNum, string letterId, string requestorId, string email, CorrMethod ecorrMethod, string fileName, EcorrData.CorrespondenceFormat format) :
            this(ssn, addrAcctNum, letterId, requestorId, email, ecorrMethod, fileName, format, DataAccessHelper.CurrentRegion)
        {
        }

        /// <summary>
        /// Letters Constructor
        /// </summary>
        public DocumentProperties(string ssn, string addrAcctNum, string letterId, string requestorId, string email, CorrMethod ecorrMethod, string fileName, EcorrData.CorrespondenceFormat format, DataAccessHelper.Region region)
        {
            DateTime currentDate = DateTime.Now;
            ADDR_ACCT_NUM = addrAcctNum;
            SSN = ssn;
            DOC_DATE = currentDate;
            LETTER_ID = letterId;
            REQUEST_USER = requestorId;
            LOAD_TIME = currentDate;
            ADDRESSEE_EMAIL = email;
            CREATE_DATE = currentDate;
            CORR_METHOD = TranslateCorrMethod(ecorrMethod, format != EcorrData.CorrespondenceFormat.Standard);
            PATH = fileName;
            CorrespondenceFormatId = (int)format;
            this.Region = region;
        }

        /// <summary>
        /// Billing constructor
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="addrAcctNum"></param>
        /// <param name="letterId"></param>
        /// <param name="requestorId"></param>
        /// <param name="email"></param>
        /// <param name="ecorrMethod"></param>
        /// <param name="fileName"></param>
        /// <param name="duedate">Must match Due Date in Compass for the bill</param>
        /// <param name="totalDue"></param>
        /// <param name="billType"></param>
        /// <param name="billSeq"></param>
        /// <param name="createDate">Must match create date for bill in compass</param>
        private DocumentProperties(string ssn, string addrAcctNum, string letterId, string requestorId, string email, CorrMethod ecorrMethod, string fileName, DateTime duedate, string totalDue, string billSeq, DateTime createDate, EcorrData.CorrespondenceFormat format)
        {
            SSN = ssn;
            DOC_DATE = DateTime.Now;
            ADDR_ACCT_NUM = addrAcctNum;
            LETTER_ID = letterId;
            REQUEST_USER = requestorId;
            LOAD_TIME = createDate;
            ADDRESSEE_EMAIL = email;
            CREATE_DATE = createDate;
            DUE_DATE = duedate;
            TOTAL_DUE = totalDue;
            BILL_SEQ = billSeq.PadLeft(4, '0');
            CORR_METHOD = TranslateCorrMethod(ecorrMethod, format != EcorrData.CorrespondenceFormat.Standard);
            PATH = fileName;
            CorrespondenceFormatId = (int)format;
            Region = DataAccessHelper.CurrentRegion;
        }

        public void InsertEcorrInformation()
        {
            if (Region == DataAccessHelper.Region.CornerStone)
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    SSN.ToSqlParameter("SSN"),
                    new SqlParameter("DOC_DATE", DOC_DATE),
                    ADDR_ACCT_NUM.ToSqlParameter("ADDR_ACCT_NUM"),
                    LETTER_ID.ToSqlParameter("LETTER"),
                    REQUEST_USER.ToSqlParameter("REQUEST_USER"),
                    new SqlParameter("LOAD_TIME", LOAD_TIME),
                    ADDRESSEE_EMAIL.ToSqlParameter("ADDRESSEE_EMAIL"),
                    new SqlParameter("CREATE_DATE", CREATE_DATE),
                    new SqlParameter("DUE_DATE", DUE_DATE),
                    new SqlParameter("TOTAL_DUE", TOTAL_DUE),
                    BILL_SEQ.ToSqlParameter("BILL_SEQ"),
                    CORR_METHOD.ToSqlParameter("CORR_METHOD"),
                    PATH.ToSqlParameter("PATH"),
                    SqlParams.Single("Format", this.CorrespondenceFormatId)
                };

                DataAccessHelper.Execute("InsertEcorrRecord", DataAccessHelper.GetManagedConnection(DataAccessHelper.Database.ECorrFed,DataAccessHelper.CurrentMode), parms.ToArray());
            }
            else
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    SSN.ToSqlParameter("SSN"),
                    new SqlParameter("DOC_DATE", DOC_DATE),
                    ADDR_ACCT_NUM.ToSqlParameter("ADDR_ACCT_NUM"),
                    LETTER_ID.ToSqlParameter("LETTER"),
                    REQUEST_USER.ToSqlParameter("REQUEST_USER"),
                    new SqlParameter("LOAD_TIME", LOAD_TIME),
                    ADDRESSEE_EMAIL.ToSqlParameter("ADDRESSEE_EMAIL"),
                    new SqlParameter("CREATE_DATE", CREATE_DATE),
                    new SqlParameter("DUE_DATE", DUE_DATE),
                    new SqlParameter("TOTAL_DUE", TOTAL_DUE),
                    BILL_SEQ.ToSqlParameter("BILL_SEQ"),
                    CORR_METHOD.ToSqlParameter("CORR_METHOD"),
                    PATH.ToSqlParameter("PATH")
                };

                DataAccessHelper.Execute("InsertEcorrRecord", DataAccessHelper.GetManagedConnection(DataAccessHelper.Database.EcorrUheaa, DataAccessHelper.CurrentMode), parms.ToArray());
            }
        }

        /// <summary>
        /// Add record to the Ecorr database, and generates the given letter as a PDF. USE FOR BILLING DOCUMENTS
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="addrAcctNum"></param>
        /// <param name="letterId"></param>
        /// <param name="requestorId"></param>
        /// <param name="email"></param>
        /// <param name="ecorrMethod"></param>
        /// <param name="fileName"></param>
        /// <param name="tempDataFile"></param>
        /// <param name="duedate"></param>
        /// <param name="totalDue"></param>
        /// <param name="billType"></param>
        /// <param name="billSeq"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        public static string GenerateEcorrBill(string ssn, string addrAcctNum, string letterId, string requestorId, string email, CorrMethod ecorrMethod, string fileName, string tempDataFile, DateTime dueDate, string totalDue, string billType, string billSeq, DateTime createDate, EcorrData.CorrespondenceFormat format)
        {
            string fileNameAndPath = EnterpriseFileSystem.GetPath("ECORRTempLocation") + Path.GetFileName(fileName);
            string file = DocumentProcessing.GeneratePdfDocument(letterId, tempDataFile, fileNameAndPath, true, "AccountNumber");
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + fileName;
            DocumentProperties doc = new DocumentProperties(ssn, addrAcctNum, letterId, requestorId, email, ecorrMethod, path, dueDate, totalDue, billSeq, createDate, format);
            doc.InsertEcorrInformation();
            return file;
        }

        public static void GenerateEcorrBillWithPDF(string ssn, string addrAcctNum, string letterId, string requesterId, string email, CorrMethod ecorrMethod, string fileName, DateTime dueDate, string totalDue, string billSeq, DateTime billCreateDate, EcorrData.CorrespondenceFormat format)
        {
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(fileName);
            DocumentProperties doc = new DocumentProperties(ssn, addrAcctNum, letterId, requesterId, email, ecorrMethod, path, dueDate, totalDue, billSeq, billCreateDate, format);
            doc.InsertEcorrInformation();
        }

        public static void GenerateEcorrBillWithPDF(string ssn, string addrAcctNum, string letterId, string requesterId, string email, CorrMethod ecorrMethod, string fileName, DateTime dueDate, string totalDue, string billSeq, DateTime billCreateDate, EcorrData.CorrespondenceFormat format, string coBorrowerAccountNumber)
        {
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(fileName);
            DocumentProperties doc = new DocumentProperties(ssn, coBorrowerAccountNumber, letterId, requesterId, email, ecorrMethod, path, dueDate, totalDue, billSeq, billCreateDate, format);
            doc.InsertEcorrInformation();
        }
    }
}