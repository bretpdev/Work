using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class QScript : Script
    {
        public QScript(string scriptId, LinkedFile file, IArea<QScript> area, bool isFed)
            : base(scriptId, file, area, isFed)
        {}

        public static QScript CreateIfQualifies(string scriptId, LinkedFile file, IArea<QScript> area, bool isFed)
        {
            MainClassFinder finder = new MainClassFinder(file.NetworkFullPath, area.DependencyNetworkRoot);
            if (finder.Results.UsesQ) return new QScript(scriptId, file, area, isFed);
            return null;
        }
    }
}
