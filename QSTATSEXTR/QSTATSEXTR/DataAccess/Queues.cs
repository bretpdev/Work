using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSTATSEXTR
{
    static class Queues
    {
        public static readonly IReadOnlyList<string> OneLinkQueueDepartments = new List<string>()
        {
            "ALL","APP","AUX","BKP","CRC","DFT","ELG","FIN","LGL","LMN","OUT","PRE","REN","RST","SCR","SKP"
        }.AsReadOnly();

        public static readonly IReadOnlyList<string> CompassQueueDepartments = new List<string>()
        {
            "AES","ASV","AUX","BSV","CLM","INQ","LMN","LOR","OPA","OPS","PRE","SKP","SOP","SSV","UQC","SKP"
        }.AsReadOnly();
    }
}
