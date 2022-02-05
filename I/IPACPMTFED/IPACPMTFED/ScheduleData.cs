using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.WinForms;

namespace IPACPMTFED
{
    public class ScheduleData
    {
        [Required]
        public string SendingServicer { get; set; }
        [Required]
        public string ScheduleNumber { get; set; }
        [Required]
        public DateTime ScheduleDate {get;set;}
        [Hidden]
        public string FormattedScheduleDate 
        {
            get
            {
                return ScheduleDate.ToString("MMddyyyy");
            }
        }
        [Required]
        public string ScheduleAmount { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }

        public ScheduleData()
        {
            ScheduleDate = DateTime.Now;
        }
    }
}
