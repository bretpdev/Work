using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    public class OutboundData
    {
        [DbName("rowid")]
        public int NobleRowId { get; set; }
        [DbName("call_type")]
        public int CallType { get; set; }
        public string UDW_DF_SPE_ACC_ID { get; set; }
        private string accountIdentifier;
        [DbName("lm_filler2")]
        public string AccountIdentifier
        {
            get
            {
                return (accountIdentifier.IsPopulated() ? accountIdentifier.Trim().Replace(" ", "") : accountIdentifier);
            }
            set
            {
                accountIdentifier = value;
            }
        }
        [DbName("areacode")]
        public string AreaCode { get; set; }
        [DbName("phone")]
        public string PhoneNumber { get; set; }
        [DbName("appl")]
        public string CallCampaign { get; set; }
        [DbName("status")]
        public string DispositionCode { get; set; }
        [DbName("addi_status")]
        public string AdditionalDispositionCode { get; set; }
        [DbName("tsr")]
        public string AgentId { get; set; }
        [DbName("act_time")]
        public DateTime ActivityDate { get; set; }
        [DbName("act_time")]
        public string EffectiveTime { get; set; }
        [DbName("listid")]
        public string ListId { get; set; }
        [DbName("vox_file_name")]
        public string VoxFileId { get; set; }
        [DbName("IsInbound")]
        public bool IsInbound { get; set; }
        [DbName("time_connect")]
        public int CallLength { get; set; }
        [DbName("TimeACW")]
        public int TimeACW { get; set; }
        [DbName("TimeHold")]
        public int TimeHold { get; set; }
        [DbName("AgentHold")]
        public int AgentHold { get; set; }
        [DbName("Filler1")]
        public string Filler1 { get; set; }
        [DbName("Filler3")]
        public int Filler3 { get; set; }
        [DbName("Filler4")]
        public int Filler4 { get; set; }
        [DbName("d_record_id")]
        public int d_record_id { get; set; }

        [DbName("DialerField1")]
        public string DialerField1 { get; set; }
        [DbName("DialerField2")]
        public string DialerField2 { get; set; }

        [DbName("DialerField3")]
        public string DialerField3 { get; set; }

        [DbName("DialerField4")]
        public string DialerField4 { get; set; }

        [DbName("DialerField5")]
        public string DialerField5 { get; set; }

        [DbName("DialerField6")]
        public string DialerField6 { get; set; }

        [DbName("DialerField7")]
        public string DialerField7 { get; set; }

        [DbName("DialerField8")]
        public string DialerField8 { get; set; }

        [DbName("DialerField9")]
        public string DialerField9 { get; set; }

        [DbName("DialerField10")]
        public string DialerField10 { get; set; }

        [DbName("DialerField11")]
        public string DialerField11 { get; set; }

        [DbName("DialerField12")]
        public string DialerField12 { get; set; }

        [DbName("DialerField13")]
        public string DialerField13 { get; set; }

        [DbName("DialerField14")]
        public string DialerField14 { get; set; }

        [DbName("DialerField15")]
        public string DialerField15 { get; set; }

        [DbName("DialerField16")]
        public string DialerField16 { get; set; }

        [DbName("DialerField17")]
        public string DialerField17 { get; set; }

        [DbName("DialerField18")]
        public string DialerField18 { get; set; }

        [DbName("DialerField19")]
        public string DialerField19 { get; set; }

        [DbName("DialerField20")]
        public string DialerField20 { get; set; }

        [DbName("DialerField21")]
        public string DialerField21 { get; set; }

        [DbName("DialerField22")]
        public string DialerField22 { get; set; }

        [DbName("DialerField23")]
        public string DialerField23 { get; set; }

        [DbName("DialerField24")]
        public string DialerField24 { get; set; }

        [DbName("DialerField25")]
        public string DialerField25 { get; set; }

        [DbName("UDW_CoborrowerAccountNumber")]
        public string UDW_CoborrowerAccountNumber { get; set; }
        public void CleanseData()
        {
            AccountIdentifier = AccountIdentifier.IsPopulated() ? AccountIdentifier.Trim().Replace(" ", "") : AccountIdentifier;
            AccountIdentifier = AccountIdentifier.IsPopulated() ? AccountIdentifier.Trim().Replace("|", "") : AccountIdentifier;
            UDW_CoborrowerAccountNumber = UDW_CoborrowerAccountNumber.IsPopulated() ? UDW_CoborrowerAccountNumber.Trim().Replace(" ", "") : UDW_CoborrowerAccountNumber;
            AgentId = AgentId ?? "";
            EffectiveTime = EffectiveTime.IsPopulated() ? EffectiveTime.PadLeft(6, '0') : null;
            //ActivityDate = new DateTime
            //    (
            //        ActivityDate.Year, ActivityDate.Month, ActivityDate.Day,
            //        EffectiveTime.IsPopulated() ? EffectiveTime.Substring(0, 2).ToInt() : 0,
            //        EffectiveTime.IsPopulated() ? EffectiveTime.Substring(2, 2).ToInt() : 0,
            //        EffectiveTime.IsPopulated() ? EffectiveTime.Substring(4, 2).ToInt() : 0
            //    );
            PhoneNumber = AreaCode.Trim() + PhoneNumber.Trim();
            IsInbound = false;
            VoxFileId = VoxFileId.IsPopulated() ? VoxFileId.Trim() ?? "" : VoxFileId ?? "";
            AdditionalDispositionCode = AdditionalDispositionCode.IsPopulated() ? AdditionalDispositionCode.Trim() ?? "" : AdditionalDispositionCode ?? "";
        }
    }
}
