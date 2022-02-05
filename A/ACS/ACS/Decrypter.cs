using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ACS.Infrastructure;

namespace ACS
{
    public class Decrypter
    {
        /// <summary>
        /// Determine which schema to use.
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public string DecryptSSN(string ssn)
        {
            int n;
            if (int.TryParse(ssn, out n))
                return LetterWriterDecryption(ssn);
            else
                return MyLaughterDecryption(ssn);
        }


        public string MyLaughterDecryption(string ssn)
        {
            string tempSSN = string.Empty;
            Dictionary<char, char> decryptionData = CreateMyLaughterDecryptData();
            for (int index = 0; index != ssn.Length; index++)
            {
                try
                {
                    tempSSN = tempSSN + decryptionData[ssn.ToCharArray()[index]];
                }
                catch (KeyNotFoundException knfx)
                {
                    throw new KeyNotFoundException(String.Format("{0}. SSN {1} contains invalid character!", knfx.Message, ssn));
                }
            }
            return tempSSN;
        }


        /// <summary>
        /// Decryption method for ACS records
        /// </summary>
        /// <returns></returns>
        private Dictionary<char, char> CreateMyLaughterDecryptData()
        {
            Dictionary<char, char> decryptionData = new Dictionary<char, char>();
            decryptionData.Add('M', '0');
            decryptionData.Add('Y', '9');
            decryptionData.Add('L', '8');
            decryptionData.Add('A', '7');
            decryptionData.Add('U', '6');
            decryptionData.Add('G', '5');
            decryptionData.Add('H', '4');
            decryptionData.Add('T', '3');
            decryptionData.Add('E', '2');
            decryptionData.Add('R', '1');
            return decryptionData;
        }


        public string LetterWriterDecryption(string ssn)
        {
            List<LTW_Spec> decryptionData = CreateLtrWrtDecryptData();
            for (int index = 0; index != ssn.Length; index++)
            {
                decryptionData[index].DecryptedValue = decryptionData[index].DecryptionDictionary[ssn.ToCharArray()[index]].ToString();
            }
            //place in correct order 
            List<string> reorderedDecryptedData = (from d in decryptionData
                                                   orderby d.IndexForActualSSN
                                                   select d.DecryptedValue).ToList<string>();
            string tempSSN = string.Join(string.Empty, reorderedDecryptedData.ToArray());
            //return completed string 
            return tempSSN;
        }



        private List<LTW_Spec> CreateLtrWrtDecryptData()
        {
            List<LTW_Spec> decryptionData = new List<LTW_Spec>();


            //decryption dictionary for index one of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("8761249305"), 6));


            //decryption dictionary for index two of ssn string
            decryptionData.Add(new LTW_Spec(ToDictionary("0953871426"), 2));


            //decryption dictionary for index three of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("6584013972"), 8));

            //decryption dictionary for index four of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("2546798103"), 9));

            //decryption dictionary for index five of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("4120935687"), 3));


            //decryption dictionary for index six of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("5302184769"), 1));

            //decryption dictionary for index seven of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("3497652018"), 5));


            //decryption dictionary for index eight of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("1879305246"), 7));


            //decryption dictionary for index nine of ssn string 
            decryptionData.Add(new LTW_Spec(ToDictionary("9018726534"), 4));

            //return data structure 
            return decryptionData;
        }

        private Dictionary<char, char> ToDictionary(string sequence)
        {
            string oneThroughZero = "1234567890";
            var dict = new Dictionary<char, char>();
            for (int i = 0; i < sequence.Length; i++)
                dict.Add(sequence[i], oneThroughZero[i]);
            return dict;
        }

        public string Decrypt(string data, Dictionary<char, char> cipher)
        {
            string output = "";
            foreach (char c in data)
                output += cipher[c];
            return output;
        }
    }
}

