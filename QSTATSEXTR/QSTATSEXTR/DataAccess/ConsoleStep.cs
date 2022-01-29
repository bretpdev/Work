using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSTATSEXTR
{
    class ConsoleStep : IDisposable
    {
        readonly string message;
        public ConsoleStep(string message)
        {
            this.message = message;
            Console.WriteLine(message + "...");
        }

        public void Dispose()
        {
            Console.WriteLine(message + " - Finished");
        }
    }
}
