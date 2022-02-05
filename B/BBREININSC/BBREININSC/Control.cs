using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBREININSC
{
     public class Control
    {
        public string Account { get; set; }

        public int Primarykey { get; set; }

        public int ForiegnKey {  get; set; }

        public List<int> Sequences { get; set; }
    }
}
