using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTC
{
    class General
	{
		public ReflectionInterface RI { get; private set; }
		public string UserId { get; private set; }
		public string ScriptId { get; private set; }
		public ProcessLogRun LogRun { get; private set; }
        public DataAccess DA { get; private set; }
        public SystemCodeHelper SCH { get; private set; }
        public DemographicsSourceHelper DSH { get; private set; }
        public RejectReasonHelper RRH { get; private set; }
		/// <summary>
		/// Contains code that is used by different classes that don't share an inheritance hierarchy.
		/// </summary>
		/// <param name="ri">The instance of ReflectionInterface used by the script.</param>
		public General(ReflectionInterface ri, string userId, string scriptId, ProcessLogRun logRun, DataAccess da = null)
		{
			RI = ri;
			ScriptId = scriptId;
			LogRun = logRun;
            UserId = userId;
            DA = da;
            if (da != null)
            {
                SCH = new SystemCodeHelper(DA);
                DSH = new DemographicsSourceHelper(DA);
                RRH = new RejectReasonHelper(DA);
            }
		}
	}
}