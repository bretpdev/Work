using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace REPAYDISCL
{
    public class Borrower
    {
        public int PrintProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string SourceFile { get; set; }
        public DateTime? ArcAddedAt { get; set; }
        public DateTime? ImagedAt { get; set; }
        public DateTime? PrintedAt { get; set; }
        public DateTime? DocumentCreatedAt { get; set; }
        public List<string> LineData { get; set; }
        public string LetterId { get; set; }
        public string DocId { get; set; }
        public string FileHeader { get; set; }
        public string Arc { get; set; }

        public Borrower()
        {
            LineData = new List<string>();
        }

        /// <summary>
        /// Takes the PrintProcessingIds and converts them to a data table.
        /// </summary>
        /// <param name="borrowers">List of borrowers</param>
        /// <returns>Datatable with 1 column PrintprocessingId</returns>
        public static DataTable GetPrintProcessingIds(List<Borrower> borrowers)
        {
            return borrowers.Select(p => new { PrintProcessingId = p.PrintProcessingId }).ToDataTable();
        }
    }
}
