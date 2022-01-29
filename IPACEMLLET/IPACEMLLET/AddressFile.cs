using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace IPACEMLLET
{
    public abstract class AddressFile
    {
        public abstract string FilePathAndName { get; }
        protected bool IsInRecovery { get; set; }
        protected ReflectionInterface RI;
        protected string Header
        {
            get
            {
                return "Keyline,AccountNumber,FirstName,LastName,Address1,Address2,City,State,Zip,Country";
            }
        }

        public AddressFile(ReflectionInterface ri, bool isInRecovery)
        {
            RI = ri;
            IsInRecovery = isInRecovery;
        }

        protected string[] GetValuesForFile(string ssn)
        {
            string keyline = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

            Func<int, int, int, string> get = new Func<int, int, int, string>((row, col, len) => RI.GetText(row, col, len).Replace("_", ""));

            //Screen is TX1J
            return new string[] { 
                        keyline, 
                        get(3, 34, 12).Replace(" ", ""),//account number
                        get(4, 34, 12),//First name
                        get(4, 6, 22),//last name
                        get(11, 10, 30),//address1
                        get(12, 10, 30),//address2
                        get(14, 8, 20),//city
                        get(14, 32, 2),//state
                        get(14, 40, 5),//zip
                        get(13, 52, 25)//foreign country
                    };

        }

        public abstract void Add(BorrowerData bor);
        public void Delete()
        {
            File.Delete(this.FilePathAndName);
        }
        public List<string> GetAccountNumbersFromFile(string file)
        {
            List<string> accountNumbers = new List<string>();
            using (StreamReader sr = new StreamReader(file))
            {
                //read out the header
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    accountNumbers.Add(sr.ReadLine().SplitAndRemoveQuotes(",")[1]);
                }
            }

            return accountNumbers;
        }
        
    }
}
