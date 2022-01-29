using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace _StarterNSF
{
    class Program
    {
        static void Main(string[] args)
        {
            ReflectionInterface ri = new ReflectionInterface(true);
            System.Windows.Forms.MessageBox.Show("Log in and press INSERT");
            ri.PauseForInsert();

            new NSFREVENTR.NSFReversalEntry(ri).Main();

            ri.CloseSession();
        }
    }
}
