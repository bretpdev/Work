using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MDLetters
{
    class LetterData
    {
        public string Description { get; set; }
        public string LetterId { get; set; }
        public bool Select { get; set; }

        public static List<LetterData> Populate()
        {
            List<LetterData> letters = DataAccessHelper.ExecuteList<LetterData>("GetAllFedLetters", DataAccessHelper.Database.ECorrFed);


            foreach (LetterData letter in letters)
            {
                try
                {
                    letter.Description = DataAccessHelper.ExecuteSingle<string>("GetLetterDescriptionFromLetterId", DataAccessHelper.Database.Bsys, SqlParams.Single("LetterId", letter.LetterId));
                }
                catch (InvalidOperationException ex)
                {
                    ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Letter {0} is in the EcorrFed Letters table but not in Letter Tracking.", letter.LetterId), NotificationType.ErrorReport, NotificationSeverityType.Warning, Program.LogData.ExecutingAssembly, ex);
                }
            }

            return letters.Where(p => !p.Description.IsNullOrEmpty()).ToList();
        }
    }
}
