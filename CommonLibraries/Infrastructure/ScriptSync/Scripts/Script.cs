using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class Script
    {
        public string ScriptId { get; set; }
        public LinkedFile File { get; set; }
        public bool IsFed { get; internal set; }
        public IArea<Script> Area {get;set;}
        public virtual bool InSync { get { return File.InSync; } }
        public virtual void Sync()
        {
            File.Sync();
        }
        public Script(string scriptId, LinkedFile file, IArea<Script> area, bool isFed)
        {
            ScriptId = scriptId;
            File = file;
            IsFed = isFed;
            Area = area;
        }

        protected string ReplaceFileName(string fullPath, string newName)
        {
            string parent = Directory.GetParent(fullPath).FullName;
            return Path.Combine(parent, newName);
        }
    }
}
