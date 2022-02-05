using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHRPT_Batch
{
    public class CsvResult
    {
        public int RowCount { get; set; }
        public string GeneratedCsv { get; set; }

        public CsvResult(int rowCount, string generatedCsv)
        {
            RowCount = rowCount;
            GeneratedCsv = generatedCsv;
        }
    }
}
