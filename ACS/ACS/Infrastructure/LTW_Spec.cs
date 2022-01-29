using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Generic;

namespace ACS
{
    public class LTW_Spec
    {
        public Dictionary<char, char> DecryptionDictionary { get; set; }
        public int IndexForActualSSN { get; set; }
        public string DecryptedValue { get; set; }

        public LTW_Spec(Dictionary<char, char> decryptDictionary, int indexForActual)
        {

            DecryptionDictionary = decryptDictionary;
            IndexForActualSSN = indexForActual;
            DecryptedValue = string.Empty;
        }
    }
}
