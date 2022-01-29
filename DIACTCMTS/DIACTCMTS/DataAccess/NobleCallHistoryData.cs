using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace DIACTCMTS
{
    class NobleCallHistoryData
    {
        public int NobleCallHistoryId { get; set; }
        public string AccountIdentifier { get; set; }
        public string ListId { get; set; }
        public string Campaign { get; set; }
        public DateTime ActivityDate { get; set; }
        public string PhoneNumber { get; set; }
        public string AgentId { get; set; }
        public string DispositionCode { get; set; }
        public string AdditionalDispositionCode { get; set; }
        public int RegionId { get; set; }
        public string CoborrowerAccountNumber { get; set; }
    }
}