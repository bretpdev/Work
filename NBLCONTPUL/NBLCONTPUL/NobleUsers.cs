using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
namespace NBLCONTPUL
{
	public class NobleUsers
	{
		[DbName("Username")]
		public string Username { get; set; }
		[DbName("TSR")]
		public string TSR { get; set; }
		[DbName("last_update")]
		public string last_update { get; set; }
	}
}
