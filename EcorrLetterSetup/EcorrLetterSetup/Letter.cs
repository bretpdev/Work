using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace EcorrLetterSetup
{
    public abstract class Letter
    {
        
        public LetterSearchData LetterData { get; set; }

        public Letter(LetterSearchData letter)
        {
            LetterData = letter;
        }

        public abstract void AddRecord(string letterId);
        public abstract dynamic GetData();
        public abstract dynamic GetDataForPromotion();
        public abstract void ChangeRecord(string letterId, dynamic selectedData);
    }
}
