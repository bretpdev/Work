using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace Uheaa
{
    public interface IArea<out T> where T : Script //this makes the interface covariant (ex List<string> is also List<object>)
    {
        string NetworkLocation { get; set; }
        string LocalLocation { get; set; }
        bool IsFed { get; set; }
        IEnumerable<T> FindAllScripts();
        string DependencyNetworkRoot { get; }
        string DependencyLocalRoot { get; }
        IEnumerable<LinkedFile> DependencyFiles { get; }
        void SyncDependencies();
    }
    public abstract class Area<T> : IArea<T>
        where T : Script
    {
        public string NetworkLocation { get; set; }
        public string LocalLocation { get; set; }
        public bool IsFed { get; set; }
        public Area(string networkLocation, string localLocation, string dependencyNetworkRoot, string dependencyLocalRoot, bool isFed)
        {
            NetworkLocation = networkLocation;
            LocalLocation = localLocation;
            DependencyNetworkRoot = dependencyNetworkRoot;
            DependencyLocalRoot = dependencyLocalRoot;
            IsFed = isFed;
            EnsureLocalFolderExists();
        }

        private void EnsureLocalFolderExists()
        {
            string folder = Path.GetDirectoryName(LocalLocation);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public abstract T FindScript(string scriptId);
        public abstract IEnumerable<T> FindAllScripts();

        public abstract IEnumerable<LinkedFile> DependencyFiles { get; }
        public string DependencyNetworkRoot { get; internal set; }
        public string DependencyLocalRoot { get; internal set; }
        public virtual void SyncDependencies()
        {
            foreach (LinkedFile file in DependencyFiles)
                file.Sync();
        }
    }
}
