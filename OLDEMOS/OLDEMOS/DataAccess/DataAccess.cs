using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace OLDEMOS
{
    public class DataAccess
    {
        [UsesSproc(MauiDude, "GetCountries")]
        public List<Country> GetCountries() =>
            ModeHelper.LogRun.LDA.ExecuteList<Country>("GetCountries", MauiDude).Result;

        [UsesSproc(Odw, "GetDemographicsSources")]
        public List<Sources> GetSources() =>
            ModeHelper.LogRun.LDA.ExecuteList<Sources>("GetDemographicsSources", Odw).Result;
    }
}