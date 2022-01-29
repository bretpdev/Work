using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SessionTester
{
    public class FakeDll : iDll
    {
        public bool IsValid { get { return false; } }
        public string DisplayName { get { return ""; } }
        public Visibility DeleteVisible { get { return Visibility.Hidden; } }
    }
}
