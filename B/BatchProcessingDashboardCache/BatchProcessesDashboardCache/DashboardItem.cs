using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessesDashboardCache
{
    class DashboardItem
    {
        public int DashboardItemId { get; set; }
        public string ItemName { get; set; }
        public string UheaaSprocName { get; set; }
        public string UheaaDatabase { get; set; }
        public string CornerstoneSprocName { get; set; }
        public string CornerstoneDatabase { get; set; }
        public bool Retired { get; set; }
    }
}
