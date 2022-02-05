using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoginDatabase
{
    public class BatchPasswordData
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public int LoginTypeId { get; set; }
        public string LoginType { get; set; }
        public string Notes { get; set; }
    }
}
