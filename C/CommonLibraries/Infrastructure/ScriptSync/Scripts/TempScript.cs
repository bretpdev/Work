using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class TempScript : Script
    {
        public TempScript(string scriptId, LinkedFile file, IArea<TempScript> area, bool isFed)
            : base(scriptId, file, area, isFed)
        {
            this.Sync();
        }

        public static TempScript CreateIfQualifies(string scriptId, LinkedFile file, IArea<TempScript> area, bool isFed)
        {
            MainClassFinder finder = new MainClassFinder(file.NetworkFullPath, area.DependencyNetworkRoot);
            if (finder.Results.UsesCommon) return new TempScript(scriptId, file, area, isFed);
            return null;
        }
    }
}
