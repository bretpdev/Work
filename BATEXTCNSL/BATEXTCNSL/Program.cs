using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATEXTCNSL
{
    class Program
    {
        static void Main(string[] args)
        {
            string header = ExitCounselingRecord.GenerateHeader();
            Console.WriteLine(header);
            Console.ReadKey();
        }
    }
}
