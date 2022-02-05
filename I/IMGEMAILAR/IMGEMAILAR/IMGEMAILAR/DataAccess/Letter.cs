using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGEMAILAR
{
    public class Letter
    {
        public string LetterId { get; set; }
        public string OverrideDescription { get; set; }

        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(LetterId))
                    return "";
                if (string.IsNullOrEmpty(OverrideDescription))
                    return LetterId;
                return LetterId + " (" + OverrideDescription + ")";
            }
        }
    }
}
