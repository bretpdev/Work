using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryCompressor
{
    abstract class Log
    {
        protected abstract void HandleWrite(string formattedText);
        public void Write(string text, params object[] formatParameters)
        {
            text = string.Format(text, formatParameters);
            text = Timestamp(text);
            HandleWrite(text);
        }

        protected int foundFileCount = 0;
        protected virtual void HandleFoundFile() { }
        public void FoundFile()
        {
            foundFileCount++;
            HandleFoundFile();
        }

        public virtual void DoneFindingFiles() { }

        protected string Timestamp(string text)
        {
            return DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + " " + text;
        }
    }
}
