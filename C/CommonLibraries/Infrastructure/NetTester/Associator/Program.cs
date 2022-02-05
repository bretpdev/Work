using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Associator
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryHelper.SetAssociation(".nt", "NetTester_Files", args.First(), "NetTester Files");
        }
    }
}
