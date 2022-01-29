using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PRIDRCRP
{
    public abstract class FileInformation
    {
        //class properties
        public abstract FileParser.FileSection Section { get; protected set; }
        public abstract List<Error> ExceptionLog { get; protected set; }

        //abstract methods
        public abstract void GetInformation(string line);
        public abstract bool FoundInformation();
        public abstract bool ValidateInformation(string file, DataAccess DA);
        public abstract bool WriteToDatabase(DataAccess DA);
        //implemented methods

        /// <summary>
        /// Finds a field based off of a header and an offset from the end of that header
        /// data must be on the same line
        /// </summary>
        protected string ParseField(string line, string header, int fieldLength, List<string> filterStrings = null)
        {
            string lineUpper = line.ToUpper();
            if (lineUpper.Contains(header))
            {
                int headerLen = header.Length;
                int headerInd = lineUpper.IndexOf(header);
                string fieldData = StringParsingHelper.SafeSubStringTrimmed(line,headerInd + headerLen, fieldLength);
                if (filterStrings != null)
                {
                    foreach (string str in filterStrings)
                    {
                        fieldData = fieldData.Replace(str, "");
                    }
                }
                return fieldData;
            }
            else
            {
                return null;
            }
        }

    }
}
