using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public abstract class DependencyScript : Script
    {
        public abstract IEnumerable<Script> Dependencies { get; }
        public override bool InSync { get { return base.InSync && !Dependencies.Any(d => !d.InSync); } }

        public override void Sync()
        {
            foreach (Script s in Dependencies)
                s.Sync();
            base.Sync();
        }

        public DependencyScript(string scriptId, LinkedFile file, IArea<Script> area, bool isFed)
            : base(scriptId, file, area, isFed) { }
    }
}
