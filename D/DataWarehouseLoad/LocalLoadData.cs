using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseLoad
{
    class LocalLoadData
    {
        public int LocalLoadDataID { get; set; }
        public string LocalLoadFile { get; set; }
        public int ReportNumber { get; set; }
        public string SasCodeName { get; set; }
        public int TryCount { get; set; }

        public LocalLoadData()
        {
            TryCount = 0;
        }

        public string GetFileName()
        {
            return string.Format("{0}{1}.*", LocalLoadFile, ReportNumber);
        }
    }
}
