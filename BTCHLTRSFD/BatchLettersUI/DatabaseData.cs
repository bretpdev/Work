using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;
using Uheaa.Common.DataAccess;


namespace BatchLettersUI
{
    public class DatabaseData
    {
        [Hidden, PrimaryKey]
        public int BatchLettersFedId { get; set; }
        [Required]
        public string LetterId { get; set; }
        [Required]
        public string SasFilePattern { get; set; }
        [Required]
        public string StateFieldCodeName { get; set; }
        [Required]
        public string AccountNumberFieldName { get; set; }
        [Required]
        public int AccountNumberFieldIndex { get; set; }
        public string CostCenterFieldCodeName { get; set; }

        public bool OkIfMissing { get; set; }
        public bool ProcessAllFiles { get; set; }
        public string Arc { get; set; }
        [TextBoxLines(3)]
        public string Comment { get; set; }
        [Hidden, DbIgnore]
        public DateTime? CreatedAt { get; set; }
        [Hidden, InsertOnly]
        public string CreatedBy { get; set; }
        [Hidden, DbIgnore]
        public DateTime? UpdatedAt { get; set; }
        [Hidden, UpdateOnly]
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
        public int BorrowerSsnIndex { get; set; }
        public bool DoNotProcessEcorr { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseData()
        {
            Active = true;
        }

        /// <summary>
        /// This constructor should be used if you want to copy the object
        /// </summary>
        /// <param name="db">DatabaseData object to copy</param>
        public DatabaseData(DatabaseData db)
        {
            BatchLettersFedId = db.BatchLettersFedId;
            LetterId = db.LetterId;
            SasFilePattern = db.SasFilePattern;
            StateFieldCodeName = db.StateFieldCodeName;
            AccountNumberFieldName = db.AccountNumberFieldName;
            CostCenterFieldCodeName = db.CostCenterFieldCodeName;
            OkIfMissing = db.OkIfMissing;
            ProcessAllFiles = db.ProcessAllFiles;
            Arc = db.Arc;
            Comment = db.Comment;
            CreatedAt = db.CreatedAt;
            CreatedBy = db.CreatedBy;
            UpdatedAt = db.UpdatedAt;
            UpdatedBy = db.UpdatedBy;
            Active = db.Active;
        }
    }
}
