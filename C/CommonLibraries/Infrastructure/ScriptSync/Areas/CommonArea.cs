using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class CommonArea : Area<CommonScript>
    {
        public CommonArea(string networkLocation, string localLocation, string dependencyNetworkRoot, string dependencyLocalRoot, bool isFed)
            : base(networkLocation, localLocation, dependencyNetworkRoot, dependencyLocalRoot, isFed) { }
        public override CommonScript FindScript(string scriptId)
        {
            string folder = Directory.GetDirectories(NetworkLocation, scriptId).FirstOrDefault();
            if (!string.IsNullOrEmpty(folder))
                return CommonScript.CreateIfQualifies(scriptId, new LinkedFile(NetworkLocation, LocalLocation, scriptId), this, IsFed);
            return null;
        }

        public override IEnumerable<CommonScript> FindAllScripts()
        {
            foreach (string folder in Directory.GetDirectories(NetworkLocation))
            {
                var script = FindScript(Path.GetFileName(folder));
                if (script != null)
                    yield return script;
            }
        }

        public override IEnumerable<LinkedFile> DependencyFiles { get { return Enumerable.Empty<LinkedFile>(); } }
    }

}
