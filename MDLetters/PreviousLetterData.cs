using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDLetters
{
    public class PreviousLetterData
    {
        public int DocumentDetailsId { get; set; }
        public string Description { get; set; }
        public string LetterId { get; set; }
        public DateTime SentAt { get; set; }
        public bool Select { get; set; }

        public static List<PreviousLetterData> PopulatePreviousLetters(string accountNumber)
        {
            List<PreviousLetterData> letters = DataAccessHelper.ExecuteList<PreviousLetterData>("GetSentLettersForBorrower", DataAccessHelper.Database.ECorrFed, SqlParams.Single("AccountNumber", accountNumber));

            foreach (PreviousLetterData letter in letters)
                letter.Description = DataAccessHelper.ExecuteSingle<string>("GetLetterDescriptionFromLetterId", DataAccessHelper.Database.Bsys, SqlParams.Single("LetteriD", letter.LetterId));  

            return letters;
        }
    }
}
