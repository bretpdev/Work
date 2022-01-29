using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace QKILLER
{
	public static class DataAccess
	{
		/// <summary>
		/// Hits bsys and gets a list of qKillerQueues
		/// </summary>
		/// <returns>List of Queue/Department pairs</returns>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetQKillerQueues")]
		public static List<WorkQueue> GetQueues()
		{
			return DataAccessHelper.ExecuteList<WorkQueue>("GetQKillerQueues", DataAccessHelper.Database.Bsys);
		}
	}
}
