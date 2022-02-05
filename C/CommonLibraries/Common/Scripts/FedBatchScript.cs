using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
	public class FedBatchScript : BatchScript
	{
		public FedBatchScript(ReflectionInterface ri, string scriptId, string errorReportFileSystemKey, string eojFileSystemKey, IEnumerable<string> eojFields)
			: base(ri, scriptId, errorReportFileSystemKey, eojFileSystemKey, eojFields, DataAccessHelper.Region.CornerStone)
		{}
	}
}
