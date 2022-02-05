using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Reflection;

namespace ReflectionValidator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(params string[] args)
        {
            try
            {
                Session ses = (Session)Microsoft.VisualBasic.Interaction.GetObject(args[0], null);
                ses.BDTIgnoreScrollLock = 1;
                Marshal.ReleaseComObject(ses);
            }
            catch (COMException)
            {
                return -1;
            }
            catch (Exception e)
            {
                if (!e.Message.StartsWith("Cannot create ActiveX component."))
                    MessageBox.Show(e.ToString());
                return -1;
            }
            return 0;
        }
    }
}
