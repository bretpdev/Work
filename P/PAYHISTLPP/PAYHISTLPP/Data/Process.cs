using System.Collections.Generic;

namespace PAYHISTLPP
{
    public class Process
    {
        public int RunId { get; set; }
        public string FileDirectory { get; set; } = null;
        public string LenderCode { get; set; }
        public bool InRecovery { get; set; }
        public bool Tilp { get; set; }
        public List<Accounts> Accounts { get; set; } = new List<Accounts>();
    }
}