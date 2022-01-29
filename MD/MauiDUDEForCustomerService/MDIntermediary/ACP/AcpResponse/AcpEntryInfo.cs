using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class AcpEntryInfo
    {
        /// <summary>
        /// This value is entered on the first screen of ACP (TCX01).
        /// </summary>
        public string AcpSelection{get;set;}
        /// <summary>
        /// This value is entered on the discussion main menu of ACP (TCX04).
        /// </summary>
        public string DiscussionOption{get;set;}
    }
}
