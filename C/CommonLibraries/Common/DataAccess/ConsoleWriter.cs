using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Uheaa.Common.DataAccess
{
    public class ConsoleWriter : TextWriter
    {
        private TextWriter originalOut = Console.Out;
        public static bool UseDefaultWriter { get; set; }

        public ConsoleWriter()
        {
        }

        public override void WriteLine(string value)
        {
            if (!UseDefaultWriter)
                originalOut.WriteLine(string.Format("Current Time: {0}; {1}", DateTime.Now, value));
            else
                originalOut.WriteLine(value);
        }

        public override void Write(string value)
        {
            originalOut.Write(value);
        }

        public override Encoding Encoding
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}
