﻿using SubSystemShared;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class MoveToOpsAccountingManager : BaseFlowCalculationProcessor
	{
		public MoveToOpsAccountingManager(TicketData theTicket)
			: base(theTicket)
		{
		}

		public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
		{
			new DataAccess(logRun);
		}
	}
}