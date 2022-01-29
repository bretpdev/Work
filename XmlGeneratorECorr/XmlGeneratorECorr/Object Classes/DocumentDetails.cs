using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace XmlGeneratorECorr
{
    class DocumentDetails
    {
        [Hidden, PrimaryKey]
        public int DocumentDetailsId { get; set; }
        [Hidden]
        public int LetterId { get; set; }
        [Ordinal(2), Manual, Required]
        public string Path { get; set; }
        [Ordinal(3), Required, MaxLength(9)]
        public string Ssn { get; set; }
        [Ordinal(4)]
        public DateTime DocDate { get; set; }
        [Ordinal(5), Required, MaxLength(10)]
        public string ADDR_ACCT_NUM { get; set; }
        [Ordinal(6), Required, MaxLength(8)]
        public string RequestUser { get; set; }
        [Ordinal(7), Required, MaxLength(20)]
        public string CorrMethod { get; set; }
        [Ordinal(8)]
        public DateTime LoadTime
        {
            get;
            set;
        }
        [Ordinal(9), Required, MaxLength(254)]
        public string AddresseeEmail { get; set; }
        [Ordinal(10)]
        public DateTime CreateDate
        {
            get;
            set;
        }
        [Ordinal(11), Label("Due Date"), DbIgnoreAttribute]
        public DateTime? DueDateAsDate
        {
            get
            {
                return DueDate.IsNullOrEmpty() ? null : DueDate.ToDateNullable();
            }
            set
            {
                DueDate = value.HasValue ? value.Value.ToShortDateString() : null;
            }
        }
        [Hidden]
        public string DueDate { get; set; }
        [Ordinal(12), MaxLength(15)]
        public string TotalDue { get; set; }
        [Ordinal(14), MaxLength(4)]
        public string BillSeq { get; set; }
        [Ordinal(15)]
        public DateTime? Printed { get; set; }
        [Manual, Required, Ordinal(17), DbIgnoreAttribute]
        public string readOnlyPath { get; set; }

        public DocumentDetails() { }
        public DocumentDetails(DateTime val)
        {
            DocDate = val;
            LoadTime = val;
            CreateDate = val;
            DueDate = val.ToShortDateString();
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateDocumentDetails")]
        public static void UpdateDb(DocumentDetails docDetailsData)
        {
            DataAccessHelper.Execute("UpdateDocumentDetails", DataAccessHelper.Database.ECorrFed, SqlParams.Update(docDetailsData));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InsertDocumentDetails")]
        public static void AddRecordDb(DocumentDetails docDetailsData)
        {
            DataAccessHelper.Execute("InsertDocumentDetails", DataAccessHelper.Database.ECorrFed, SqlParams.Insert(docDetailsData));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InactiveDocumentDetailRecord")]
        public static void InactiveRecord(int documentDetailsId)
        {
            DataAccessHelper.Execute("InactiveDocumentDetailRecord", DataAccessHelper.Database.ECorrFed, new SqlParameter("DocumentDetailsId", documentDetailsId));
        }
    }
}
