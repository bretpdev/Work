using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BILLING
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
        public string LineData { get; set; }
        public string LetterId { get; set; }
        public string DocId { get; set; }
        public string FileHeader { get; set; }
        public string ARC { get; set; }

        public int ReportNumber
        {
            get
            {
                int? reportNumber = null;
                if (SourceFile.Length >= 14 && SourceFile.Length <= 15) //SAS file does not have a date on the end of the name
                {
                    reportNumber = SourceFile.Length == 15 ? SourceFile.Substring(SourceFile.LastIndexOf("LWS14R") + 6, 2).ToIntNullable() :
                       SourceFile.Substring(SourceFile.LastIndexOf("LWS14R") + 6, 1).ToIntNullable();
                }
                else //SAS file has a date on the end of the name
                {
                    reportNumber = SourceFile.Substring(SourceFile.LastIndexOf("LWS14R") + 6, 2).ToIntNullable();
                    if (!reportNumber.HasValue)
                        reportNumber = SourceFile.Substring(SourceFile.LastIndexOf("LWS14R") + 6, 1).ToIntNullable();
                }
                return (int)reportNumber;
            }
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