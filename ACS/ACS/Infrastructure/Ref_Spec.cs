using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Infrastructure
{
    public class Ref_Spec
    {
        public string ID { get; set; }
        public string Address { get; set; }

        public Ref_Spec(string id, string address)
        {
            ID = id;
            Address = address;
        }
    }
}
