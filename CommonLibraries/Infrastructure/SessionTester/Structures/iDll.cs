using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SessionTester
{
    public interface iDll
    {
        bool IsValid { get; }
        string DisplayName { get; }
        Visibility DeleteVisible { get; }
    }
}
