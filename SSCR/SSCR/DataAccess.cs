using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace SSCR
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) => LDA = lda;

        /// <summary>
        /// Gets a list of school codes and the department for any school missing the SSCR field
        /// </summary>
        [UsesSproc(Odw, "sscr.GetAvailableSchools")]
        public List<string> GetSchools() => LDA.ExecuteList<string>("sscr.GetAvailableSchools", Odw).Result;
    }
}