using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class CommonScript : DependencyScript
    {
        private List<Script> dependencies = new List<Script>();
        public override IEnumerable<Script> Dependencies { get { return dependencies; } }
        public CommonScript(string scriptId, LinkedFile file, IArea<CommonScript> area, bool isFed)
            : base(scriptId, file, area, isFed)
        {
            LinkedFile mainDll = null;
            foreach (string filepath in Directory.GetFiles(file.NetworkFullPath))
            {
                string filename = Path.GetFileName(filepath);
                LinkedFile lf = new LinkedFile(file.NetworkFullPath, file.LocalFullPath, filename);
                if (lf.FileName.ToLower() == scriptId + ".dll")
                    mainDll = lf;
                Script d = new Script(filename, lf, area, isFed);
                dependencies.Add(d);
            }
            //fix file references
            File = mainDll;
            this.Sync();
        }

        public override void Sync()
        {
            if (Directory.Exists(File.LocalRoot))
                Directory.Delete(File.LocalRoot, true);
            Directory.CreateDirectory(File.LocalRoot);
            foreach (Script d in dependencies)
                d.Sync();
        }

        public static CommonScript CreateIfQualifies(string scriptId, LinkedFile file, IArea<CommonScript> area, bool isFed)
        {
            MainClassFinder finder = new MainClassFinder(Path.Combine(file.NetworkFullPath, file.FileName + ".dll"), area.DependencyNetworkRoot);
            if (finder.Results.UsesCommon) return new CommonScript(scriptId, file, area, isFed);
            return null;
        }
    }
}
