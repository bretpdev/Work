﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;


namespace DTX7LUHEAA
{
    public class Dtx7lDeletedRecords
    {
        public int DTX7LDeletedRecordId { get; set; }
        public string Ssn { get; set; }
        public string Arc { get; set; }
        public DateTime RequestDate { get; set; }
    }
}