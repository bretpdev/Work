using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb
{
    public class CourtHelper
    {
        private string fullCourtName;
        public CourtHelper(UserHelper uh)
        {
            this.fullCourtName = uh.CurrentFullName;
        }
        [UsesSproc(DataAccessHelper.Database.Scheduler, "MoveSasToCourt")]
        public void MoveSasToCourt(int sasId)
        {
            DataAccessHelper.Execute("MoveSasToCourt", DataAccessHelper.Database.Scheduler, SqlParams.Single("RequestId", sasId), SqlParams.Single("CourtFullName", fullCourtName));
        }

        [UsesSproc(DataAccessHelper.Database.Scheduler, "MoveScriptToCourt")]
        public void MoveScriptToCourt(int scriptId)
        {
            DataAccessHelper.Execute("MoveScriptToCourt", DataAccessHelper.Database.Scheduler, SqlParams.Single("RequestId", scriptId), SqlParams.Single("CourtFullName", fullCourtName));
        }

        [UsesSproc(DataAccessHelper.Database.Scheduler, "MoveLetterToCourt")]
        public void MoveLetterToCourt(int letterId)
        {
            DataAccessHelper.Execute("MoveLetterToCourt", DataAccessHelper.Database.Scheduler, SqlParams.Single("RequestId", letterId), SqlParams.Single("CourtFullName", fullCourtName));
        }
    }
}