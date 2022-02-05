using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCompressor
{
    class ConsoleLog : Log
    {
        protected override void HandleWrite(string formattedText)
        {
            Console.WriteLine(formattedText);
        }
    }
}
