using System;
using Uheaa.Common.DataAccess;

namespace SCHRPT_Batch
{
    /// <summary>
    /// A class to store the information recieved from calls
    /// to LogDataAccess.ExecuteList
    /// </summary>
    public class SchrptRecord
    {
        [DbName("RecipientId")]
        public int RecipientId { get; set; }

        [DbName("Name")]
        public string Name { get; set; }

        [DbName("Email")]
        public string Email { get; set; }

        [DbName("CompanyName")]
        public string CompanyName { get; set; }

        [DbName("AddedOn")]
        public DateTime AddedOn { get; set; }

        [DbName("AddedBy")]
        public string AddedBy { get; set; }

        [DbName("ReportTypeId")]
        public int ReportTypeId { get; set; }

        [DbName("StoredProcedureName")]
        public string StoredProcedureName { get; set; }

        [DbName("SchoolEmailHistoryId")]
        public int SchoolEmailHistoryId { get; set; }

        [DbName("SchoolRecipientId")]
        public int SchoolRecipientId { get; set; }

        [DbName("EmailSentAt")]
        public DateTime EmailSentAt { get; set; }

        [DbName("SchoolId")]
        public int SchoolId { get; set; }

        [DbName("SchoolCode")]
        public string SchoolCode { get; set; }

        [DbName("BranchCode")]
        public string BranchCode { get; set; }
    }
}
