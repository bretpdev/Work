using System;

namespace FSAMTHCALL
{
    public class NobleData
    {
        public enum CallType
        {
            Inbound,
            OutBound,
            Special
        }

        [ExcelHeader("Call_ID_Header")]
        public int CallIdNumber { get; set; }
        [ExcelHeader("Call_Date")]
        public DateTime CallDate { get; set; }
        [ExcelHeader("Call_Length")]
        public int CallLength { get; set; }
        [ExcelHeader("Call_Type")]
        public bool IsInbound { get; set; }
        [ExcelHeader("CSR Name/Agent ID")]
        public string CSRName_AgentId { get; set; }
        public string VoxFileId { get; set; }
        public string VoxFileLocation { get; set; }
        public bool VoxFileNotFound { get; set; }
        public string CallCampaign { get; set; }
        public CallType Type { get; set; }
        [ExcelHeader("Account_Number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Calculates Hours.Minutes.Seconds from total number of Seconds
        /// </summary>
        /// <returns></returns>
        public string CalculateTime()
        {
            var timeSpan = new TimeSpan(0, 0, CallLength);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static string TranslateEnum(CallType type)
        {
            switch (type)
            {
                case CallType.Inbound:
                    return "Inbound";
               case CallType.OutBound:
                    return "Outbound";
               case CallType.Special:
                    return "Specialty Calls";
                default:
                    return null;
            }
        }
    }
}
