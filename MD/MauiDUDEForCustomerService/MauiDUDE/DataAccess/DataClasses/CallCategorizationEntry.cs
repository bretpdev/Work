using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class CallCategorizationEntry
    {
        public enum CallCategory
        {
            //SingleMoms, These no longer exist
            //UtahFutures,
            NoUHEAAConnection
        }

        /// <summary>
        /// Category for db entry
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Reason for db entry
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Letter Id for db entry
        /// </summary>
        public string LetterID { get; set; }

        /// <summary>
        /// Comments for db entry
        /// </summary>
        public string Comments { get; set; } = "";
        public string UserID { get; set; }
        public string Region { get; set; }
    }
}
