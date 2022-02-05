using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace SessionTester
{
    public class Dll : FakeDll
    {
        public string Path { get; set; }

        /// <summary>
        /// A string that looks like SCRIPTID (Debug) or SCRIPTID (Release)
        /// </summary>
        public string DisplayName { get { return string.Format("{0} ({1})", ScriptId, System.IO.Path.GetFileName(ContainingFolder)); } }

        public Dll(string path, DataAccessHelper.Region region)
        {
            Path = path;
            this.Region = region;
            this.ScriptId = System.IO.Path.GetFileName(Path).Replace(".dll", string.Empty).PadRight(10);
            this.ContainingFolder = System.IO.Path.GetDirectoryName(Path);

            AppDomain domain = AppDomain.CreateDomain("test");
            DllInfo di = (DllInfo)domain.CreateInstanceAndUnwrap(typeof(DllInfo).Assembly.FullName, typeof(DllInfo).FullName, false, BindingFlags.Default, null, new object[] { path}, null, null);
            this.IsScript = di.IsScript;
            this.IsOld = di.IsOld;
            this.ClassName = di.ClassName;
            this.IsValid = di.IsValid;
            AppDomain.Unload(domain);

            this.LastModified = new FileInfo(Path).LastWriteTime;
        }

        public DataAccessHelper.Region Region { get; set; }
        public string ClassName { get; internal set; }
        public bool IsScript { get; internal set; }
        public string ScriptId { get; internal set; }
        public bool IsOld { get; internal set; }
        public bool IsValid { get; internal set; }
        public Visibility DeleteVisible { get { return Visibility.Visible; } }
        public DateTime LastModified { get; internal set; }

        public string ContainingFolder { get; internal set; }
        public void DeleteContainingFolder()
        {

            foreach (string file in Directory.GetFiles(ContainingFolder))
                System.IO.File.Delete(file);
            Directory.Delete(ContainingFolder);
        }
    }
}
