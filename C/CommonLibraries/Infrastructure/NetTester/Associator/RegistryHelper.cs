using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Associator
{
    static class RegistryHelper
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static void SetAssociation(string extension, string keyName, string openWith, string fileDescription)
        {
            RegistryKey baseKey = Registry.ClassesRoot.CreateSubKey(extension);
            baseKey.SetValue("", keyName);

            RegistryKey openMethod = Registry.ClassesRoot.CreateSubKey(keyName);
            openMethod.SetValue("", fileDescription);
            openMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + openWith + "\",0");

            RegistryKey shell = openMethod.CreateSubKey("Shell");
            shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + openWith + "\"" + " \"%1\"");
            shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + openWith + "\"" + " \"%1\"");
            baseKey.Close();
            openMethod.Close();
            shell.Close();

            RegistryKey currentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension, true);
            if (currentUser != null)
            {
                currentUser.DeleteSubKey("UserChoice", false);
                currentUser.Close();
            }

            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
