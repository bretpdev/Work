using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class LinkedFile
    {
        public string NetworkRoot { get; set; }
        public string LocalRoot { get; set; }
        public string FileName { get; set; }

        public string NetworkFullPath { get { return Path.Combine(NetworkRoot, FileName); } }
        public string LocalFullPath { get { return Path.Combine(LocalRoot, FileName); } }

        public bool InSync { get { return File.GetLastWriteTimeUtc(NetworkFullPath) == File.GetLastWriteTimeUtc(LocalFullPath); } }

        public void Sync()
        {
            if (!InSync)
            {
                if (File.Exists(LocalFullPath))
                    File.Delete(LocalFullPath);
                if (!Directory.Exists(LocalRoot))
                    Directory.CreateDirectory(LocalRoot);
                File.Copy(NetworkFullPath, LocalFullPath);
            }
        }

        public LinkedFile(string networkRoot, string localRoot, string fileName)
        {
            NetworkRoot = networkRoot;
            LocalRoot = localRoot;
            FileName = fileName;
        }
    }
}
