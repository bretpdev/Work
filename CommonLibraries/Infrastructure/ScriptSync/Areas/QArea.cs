using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class QArea : Area<QScript>
    {
        public QArea(string networkLocation, string localLocation, string dependencyNetworkRoot, string dependencyLocalRoot, bool isFed) 
            : base(networkLocation, localLocation, dependencyNetworkRoot, dependencyLocalRoot, isFed) { }
        public override QScript FindScript(string scriptId)
        {
            string file = Directory.GetFiles(NetworkLocation, scriptId + ".dll").FirstOrDefault();
            if (!string.IsNullOrEmpty(file))
                return QScript.CreateIfQualifies(scriptId, new LinkedFile(NetworkLocation, LocalLocation, Path.GetFileName(file)), this, IsFed);
            return null;
        }

        public override IEnumerable<QScript> FindAllScripts()
        {
            foreach (string file in Directory.GetFiles(NetworkLocation).Select(o => Path.GetFileNameWithoutExtension(o)).Distinct())
            {
                var script = FindScript(file);
                if (script != null)
                    yield return script;
            }
        }

        public override IEnumerable<LinkedFile> DependencyFiles
        {
            get { yield return new LinkedFile(DependencyNetworkRoot, DependencyLocalRoot, "Q.dll"); }
        }
    }
}
