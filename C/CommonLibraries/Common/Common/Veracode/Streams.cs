using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class StreamW : System.IO.StreamWriter
    {
        public StreamW(string path) : this(path, false) { }
        public StreamW(string path, bool append) : base(path, append)
        {
            FS.ValidatePath(path);
        }
    }

    public class StreamR : System.IO.StreamReader
    {
        public StreamR(string path) : base(path)
        {
            FS.ValidatePath(path);
        }
    }

    public class FStream : System.IO.FileStream
    {
        public FStream(string path, FileMode mode) : base(path, mode)
        {
            FS.ValidatePath(path);
        }
        public FStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
            FS.ValidatePath(path);
        }
    }
}
