using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class SerfFileBase
    {
        public static ReaderWriterLock rwl = new ReaderWriterLock();

        public static string GetHeaderInformation(string owner, string ssn, string recordType, string seqNumber)
        {
            return string.Format("{0}{1}00000{2}{3}{4}", "".PadLeft(10, ' '), owner, ssn, recordType, seqNumber);
        }

        public bool CheckLength(int length, string value)
        {
            return value.Length == length;
        }

        public string ConvertAlignDate(string date)
        {
            return string.Format("{0}/{1}/{2}{3}", date.Substring(3,2), date.Substring(5,2), date.Substring(0,1) == "0" ? "19" : "20" , date.Substring(1,2));
        }

        public virtual void WriteRecord(string file, string ssn)
        { }
    }
}
