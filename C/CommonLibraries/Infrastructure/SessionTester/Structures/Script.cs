using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionTester
{
    public class Script
    {
        public string Id { get; set; }
        public IEnumerable<Dll> Dlls { get; set; }
        public Script(string id, IEnumerable<Dll> dlls)
        {
            this.Id = id;
            this.Dlls = dlls;
        }
        public void Delete()
        {
            foreach (Dll dll in Dlls)
                dll.DeleteContainingFolder();
        }
    }
}
