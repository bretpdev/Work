using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace ECORRINERR
{
    public class ErrorData
    {
        public string path { get; set; }
        public string SSN { get; set; }
        public string DOC_DATE{ get; set; }
        public string DOC_ID { get; set; }
        public string ADDR_ACCT_NUM { get; set; }
        public string LETTER_ID { get; set; }
        public string REQUEST_USER { get; set; }
        public string VIEWABLE { get; set; }
        public string CORR_METHOD { get; set; }
        public string REPORT_DESC { get; set; }
        public string LOAD_TIME { get; set; }
        public string REPORT_NAME { get; set; }
        public string ADDRESSEE_EMAIL { get; set; }
        public string VIEWED { get; set; }
        public string CREATE_DATE { get; set; }
        public string MAINFRAME_REGION { get; set; }
        public string DCN { get; set; }
        public string SUBJECT_LINE { get; set; }
        public string DOC_SOURCE { get; set; }
        public string DOC_COMMENT { get; set; }
        public string WORKFLOW { get; set; }
        public string DOC_DELETE { get; set; }
        public string Region { get; set; }
        public string ErrorText { get; set; }
        public string ErrorFileName { get; set; }
    }
}