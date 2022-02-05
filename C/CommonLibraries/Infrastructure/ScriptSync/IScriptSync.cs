using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa
{
    [Guid("A141FBA1-67A1-41BB-942A-E249F483B248")]
    [ComVisible(true)]
    public interface IScriptSync
    {
        [ComVisible(true)]
        bool SyncAndStartWithErrorPopup(Reflection.Session session, int mode, string scriptId);
        [ComVisible(true)]
        bool SyncAndStart(Reflection.Session session, int mode, string scriptId, bool errorPopup = true);
    }
}
