using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace MdSession
{
    public class ScriptInfo
    {
        public string ScriptName { get; set; }
        public Image Icon { get; set; }
        public string ScriptId { get; set; }
        public string StartingType { get; set; }
        public bool IsCSharp { get; set; }
        public bool IsLocal { get; set; }
        public ScriptInfo(string scriptName, Image icon, string scriptId, string startingType, bool isCSharp, bool isLocal)
        {
            ScriptName = scriptName;
            Icon = icon;
            ScriptId = scriptId;
            StartingType = startingType;
            IsCSharp = isCSharp;
            IsLocal = isLocal;
        }

        public void LaunchScript(Reflection.Session ri)
        {
            if (!IsLocal)
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                ri.OLEServerName = guid;
#if DEBUG
                Proc.Start("MDScriptCoDebug", $"datamode:{(ModeHelper.TestMode ? "dev" : "live")} scriptid:{ScriptId} reflectionolename:{guid} classname:{StartingType}");
#else
                Proc.Start("MDScriptCo", $"datamode:{(ModeHelper.TestMode ? "dev" : "live")} scriptid:{ScriptId} reflectionolename:{guid} classname:{StartingType}");
#endif
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();
                string localLocation = assembly.Location;
                if (localLocation != null)
                {
                    string localParent = Path.GetDirectoryName(localLocation);
                    AppDomain dom = AppDomain.CreateDomain("ScriptLauncher", null, localParent, localParent, true);
                    object[] args = new object[] { localLocation, ScriptId, StartingType, ri.OLEServerName, IsCSharp, ModeHelper.TestMode };
                    dom.CreateInstanceFrom(assembly.Location, typeof(ScriptLauncher).FullName, true, BindingFlags.Default, null, args, null, null);
                    AppDomain.Unload(dom); //unload assembly so we can free the .dll again
                }
            }
        }

        private static string FfelScriptLocation { get { return ModeHelper.TestMode ? @"X:\PADU\UHEAACodeBase\" : @"X:\Sessions\UHEAA CodeBase\"; } }
    }
}
