using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
	static class ScriptHelper
	{
		public static string GetScriptName(string scriptId)
		{
            DataContext context = DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys, DataAccessHelper.Mode.Live);
            return context.ExecuteQuery<string>("SELECT Script FROM SCKR_DAT_Scripts WHERE ID = {0}", scriptId).SingleOrDefault();
		}
	}
}
