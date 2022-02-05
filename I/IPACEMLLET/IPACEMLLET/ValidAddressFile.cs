using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;

namespace IPACEMLLET
{
    public class ValidAddressFile : AddressFile
    {
        public override string FilePathAndName
        {
            get
            {
                return Path.Combine(EnterpriseFileSystem.TempFolder, "LetterDataFile.txt");
            }
        }

        public ValidAddressFile(ReflectionInterface ri, bool isInRecovery)
            : base(ri, isInRecovery)
        {
            using (StreamWriter sw = new StreamWriter(this.FilePathAndName))
                sw.WriteLine(this.Header);
        }

        /// <summary>
        /// Adds the given borrower to temp file for printing
        /// </summary>
        /// <param name="bor">Current Borrower object.</param>
        public override void Add(BorrowerData bor)
        {
            using (StreamWriter sw = new StreamWriter(this.FilePathAndName, true))
                sw.WriteLine(string.Join(",", GetValuesForFile(bor.BorrowerSsn)));
        }
    }
}
