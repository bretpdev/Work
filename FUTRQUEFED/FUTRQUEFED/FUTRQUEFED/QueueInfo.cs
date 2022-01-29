using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FUTRQUEFED
{
    public class QueueInfo
    {
        public string AccountNumber { get; set; }
        public string RecipientId { get; set; }
        public string Arc { get; set; }
        public DateTime ArcAddDate { get; set; }
        public string Comment { get; set; }

        public QueueInfo()
        {
            ArcAddDate = DateTime.Today;
        }
    }
}
