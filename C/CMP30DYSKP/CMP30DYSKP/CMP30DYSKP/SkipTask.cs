using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP30DYSKP
{
    /// <summary>
    /// Encapsulates skip task data parsed from the LWK17R2 file
    /// </summary>
    public class SkipTask
    {
        public string Ssn { get; set; }
        public string Task { get; set; }
    }
}
