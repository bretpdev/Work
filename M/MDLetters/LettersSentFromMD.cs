using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDLetters
{
    class LettersSentFromMD
    {
        public string Description { get; set; }
        public string Arc { get; set; }
        public bool Select { get; set; }

        public static List<LettersSentFromMD> Populate(string homePage)
        {
            return DataAccessHelper.ExecuteList<LettersSentFromMD>("GetLettersForHomePage", DataAccessHelper.Database.MauiDude, SqlParams.Single("HomePage", homePage));
        }
    }
}
