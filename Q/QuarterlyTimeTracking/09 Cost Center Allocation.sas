/*NOTES REGARDING LIVE AND TEST*/

/*  1.	This job pulls Sacker, letter tracking, PMD, projects database, and Need Help data from live so references to CSYS.GENR_LST_BusinessUnits, CSYS.SYSA_DAT_Users, COST_DAT_BusinessUnitCostCenters, COST_DAT_CostCenters, and COST_DAT_MailCodeCostCenters must also always point to live.*/
/*  2.	COST_DAT_AgentWeights is a working table pointed to test for convenience even though the SqlUserId must be from live as the table is joined to live data*/
/*  3.	COST_DAT_BatchScriptWeights is a working table pointed to test for convenience even though the CostCenterId must be from live as the table is joined to live data*/
/*  4.	COST_DAT_TimeTracking is a working table pointed to test for convenience in loading the table even though the process populates CostCenterId and SqlUserId from live */
/*  5.  COST_DAT_NhManualAllocation is a working table pointed to test for convenience in loading the table*/

LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYL ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no"); /*CSYS live (nochouse)*/
LIBNAME CSY_TEST ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYSTEST.dsn; update_lock_typ=nolock; bl_keepnulls=no"); /*CSYS test (opsdev)*/

/*'the library references below are to production MS Access databases, sometimes SAS puts the databases (especially PJ) in an inconsistent state so you may want to use the Budget Reporting Copies lib refs below them instead'*/
LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");

/*'these library references are to copies of production MS Acess databases, be sure to update the copies in X:\PADR\Budget Reporting Copies\ before using them*/
/*LIBNAME PJ ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PJ.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/
/*LIBNAME PMD ODBC %STR(REQUIRED="FILEDSN=X:\PADR\Budget Reporting Copies\PMD.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/

LIBNAME RPT ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\Reporting.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME NHU ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpUheaa.dsn; update_lock_typ=nolock; bl_keepnulls=no");

%LET PeriodBegin = '01APR2021'D;  *first day of quarter being reported;
%LET PeriodEnd = '30APR2021'D; *last day of quarter being reported;

/*master list of Need Help time tracking records*/
PROC SQL;
	CREATE TABLE NeedHelpTimeTracking AS
		SELECT
			*
		FROM
			(
				SELECT
					TT.TimeTrackingId,
					TT.SqlUserID,
					TT.TicketID,
					NH.Subject,
					TT.StartTime,
					PUT(INPUT(SUBSTR(PUT(TT.StartTime, DATETIME22.3),1,9), DATE9.), YYMMDD10.) AS TaskDate,
					TT.EndTime,
					(TT.EndTime - TT.StartTime)/3600 AS Hours,
					'Need Help UHEAA Ticket' AS TaskType,
					NH.Unit
				FROM
					RPT.TimeTracking TT
					INNER JOIN NHU.DAT_Ticket NH
						ON TT.TicketID = NH.Ticket
						AND TT.Region = 'uheaa'
				WHERE					
					TT.EndTime IS NOT NULL
					AND TT.StartTime BETWEEN &PeriodBegin AND &PeriodEnd
					AND NH.TicketCode NOT IN ('BAC','FAC','FAR','FTRANS')
					AND UPCASE(NH.Subject) NOT LIKE ('%BUILDI%ACC%') /*remove building access tickets*/
			)
		ORDER BY
			TicketID
;QUIT;

/*get data from each tracking system or database*/
PROC SQL;
/*	Need Help (non batch script running tickets) - NH*/
	CREATE TABLE NeedHelp AS
		SELECT
			'NH' AS RecordType,
			TimeTrackingId,
/*			TaskDate,*/
			INPUT(TaskDate, YYMMDD10.) AS TaskDate FORMAT DATE9.,
			TT.Hours,
			TT.TaskType,
			TT.TicketID AS TaskNo,
			TT.Subject AS TaskDescription,
			'' AS GenericMeetings,
			CU.WindowsUserName AS Agent,
			TT.SqlUserID AS SqlUserId,
			CASE
				WHEN TT.TaskType = 'Need Help CornerStone Ticket' 
				THEN 'Need Help CornerStone Ticket'
				ELSE BU.Name 
			END AS CostCenterDeterminant,
			CASE
				WHEN TT.TaskType = 'Need Help CornerStone Ticket' 
				THEN 13
				ELSE BC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN TT.TaskType = 'Need Help CornerStone Ticket' 
					OR BC.Weight IS NULL 
				THEN 100
				ELSE BC.Weight
			END AS CostCenterWeight,

/*			TT.SqlUserID AS SqlUserId,*/
/*			CASE*/
/*				WHEN NHMA.TicketID IS NOT NULL THEN NHMA.CostCenterDeterminant*/
/*				WHEN TT.TaskType = 'Need Help CornerStone Ticket' THEN 'Need Help CornerStone Ticket'*/
/*				ELSE BU.Name */
/*			END AS CostCenterDeterminant,*/
/*			CASE*/
/*				WHEN NHMA.TicketID IS NOT NULL THEN NHMA.CostCenterId*/
/*				WHEN TT.TaskType = 'Need Help CornerStone Ticket' THEN 13*/
/*				ELSE BC.CostCenterId*/
/*			END AS CostCenterId,*/
/*			CASE*/
/*				WHEN NHMA.TicketID IS NOT NULL THEN NHMA.CostCenterWeight*/
/*				WHEN TT.TaskType = 'Need Help CornerStone Ticket' */
/*					OR BC.Weight IS NULL*/
/*				THEN 100*/
/*				ELSE BC.Weight*/
/*			END AS CostCenterWeight,*/

			AW.Weight AS AgentWeight
		FROM
			NeedHelpTimeTracking TT
			INNER JOIN CSYL.SYSA_DAT_Users CU
				ON TT.SqlUserId = CU.SqlUserID
/*				AND CU.Status = 'Active'*/
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.GENR_LST_BusinessUnits BU
				ON TT.Unit = BU.ID
			LEFT JOIN CSYL.COST_DAT_BusinessUnitCostCenters BC
				ON TT.Unit = BC.BusinessUnitId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN BC.EffectiveBegin AND COALESCE(BC.EffectiveEnd, DATE())

/*			LEFT JOIN CSY_TEST.COST_DAT_NhManualAllocation NHMA*/
/*				ON TT.TicketID = NHMA.TicketID */
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN NHMA.EffectiveBegin AND COALESCE(NHMA.EffectiveEnd, DATE())*/

		WHERE
			TT.TaskType = 'Need Help CornerStone Ticket'
			OR (
					TT.TaskType = 'Need Help UHEAA Ticket' 
					AND TT.Subject NOT LIKE ('Batch Scripts%')
				)
		ORDER BY
			TT.TimeTrackingId
	;

/*	Need Help (batch script running tickets) - batchscripts*/
	CREATE TABLE BatchScripts AS
		SELECT
			'NH' AS RecordType,
			TimeTrackingId,
/*			TaskDate,*/
			INPUT(TaskDate, YYMMDD10.) AS TaskDate FORMAT DATE9.,
			TT.Hours,
			TT.TaskType,
			TT.TicketID AS TaskNo,
			TT.Subject AS TaskDescription,
			'' AS GenericMeetings,
			CU.WindowsUserName AS Agent,
			TT.SqlUserID AS SqlUserId,
			'Need Help Batch Script Running Ticket' AS CostCenterDeterminant,
			BS.CostCenterId AS CostCenterId,
			BS.Weight AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			NeedHelpTimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_BatchScriptWeights BS
/*				ON INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BS.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BS.EffectiveEnd, YYMMDD10.), DATE())*/
				ON INPUT(TT.TaskDate, YYMMDD10.) BETWEEN BS.EffectiveBegin AND COALESCE(BS.EffectiveEnd, DATE())
			INNER JOIN CSYL.SYSA_DAT_Users CU
				ON TT.SqlUserId = CU.SqlUserID
/*				AND CU.Status = 'Active'*/
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.Subject LIKE ('Batch Scripts%')
			AND TT.TaskType = 'Need Help UHEAA Ticket'
		ORDER BY
			TT.TimeTrackingId
	;

/*	script requests - SR*/
	CREATE TABLE Scr AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Script Request' AS TaskType,
			TT.Sr AS TaskNo,
			SR.Script AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			CASE
				WHEN UPCASE(SR.Script) LIKE ('%FED') 
					OR UPCASE(SR.Script) LIKE ('%FED)') 
				THEN 'FED in Asset Name'
				WHEN SCR.IsFED = 1 
				THEN 'FED Checked on Detail Screen'
				ELSE BU.Unit
			END AS CostCenterDeterminant,
			CASE
				WHEN UPCASE(SR.Script) LIKE ('%FED') 
					OR UPCASE(SR.Script) LIKE ('%FED)') 
					OR SCR.IsFED = 1 
				THEN 13 /*CornerStone Portfolio Servicing*/
				ELSE BC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN UPCASE(SR.Script) LIKE ('%FED') 
					OR UPCASE(SR.Script) LIKE ('%FED)') 
					OR BC.Weight IS NULL 
					OR SCR.IsFED = 1 
				THEN 100.0
				ELSE BC.Weight
			END AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN BSYS.SCKR_DAT_ScriptRequests SR
				ON TT.Sr = SR.Request
			INNER JOIN BSYS.SCKR_DAT_Scripts SCR
				ON SR.Script = SCR.Script
			INNER JOIN BSYS.SCKR_REF_Unit BU
				ON SR.Script = BU.Program
			INNER JOIN CSYL.GENR_LST_BusinessUnits CU
				ON BU.Unit = CU.Name
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.COST_DAT_BusinessUnitCostCenters BC
				ON CU.ID = BC.BusinessUnitId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN BC.EffectiveBegin AND COALESCE(BC.EffectiveEnd, DATE())
		WHERE
			TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd		*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd	
		ORDER BY
			TT.TimeTrackingId
	;

	CREATE TABLE Sas AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'SAS Request' AS TaskType,
			TT.Sasr AS TaskNo,
			SR.Job AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			CASE
				WHEN UPCASE(SR.Job) LIKE ('%FED') 
					OR UPCASE(SR.Job) LIKE ('%FED)') 
				THEN 'FED in Asset Name'
				WHEN SAS.IsFED = 1 
				THEN 'FED Checked on Detail Screen'
				ELSE BU.Unit
			END AS CostCenterDeterminant,
			CASE
				WHEN UPCASE(SR.Job) LIKE ('%FED') 
					OR UPCASE(SR.Job) LIKE ('%FED)') 
					OR SAS.IsFED = 1 
				THEN 13 /*CornerStone Portfolio Servicing*/
				ELSE BC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN UPCASE(SR.Job) LIKE ('%FED') 
					OR UPCASE(SR.Job) LIKE ('%FED)') 
					OR BC.Weight IS NULL 
					OR SAS.IsFED = 1 
				THEN 100.0
				ELSE BC.Weight
			END AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN BSYS.SCKR_DAT_SASRequests SR
				ON TT.Sasr = SR.Request
			INNER JOIN BSYS.SCKR_DAT_SAS SAS
				ON SR.Job = SAS.Job
			INNER JOIN BSYS.SCKR_REF_UnitSAS BU
				ON SR.Job = BU.Program
			INNER JOIN CSYL.GENR_LST_BusinessUnits CU
				ON BU.Unit = CU.Name
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.COST_DAT_BusinessUnitCostCenters BC
				ON CU.ID = BC.BusinessUnitId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN BC.EffectiveBegin AND COALESCE(BC.EffectiveEnd, DATE())
		WHERE
			TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd	*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd	
		ORDER BY
			TT.TimeTrackingId
	;

/*	letter requests - LTS*/
	CREATE TABLE Lts AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Letter Request' AS TaskType,
			TT.Lts AS TaskNo,
			DD.DocName AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			CASE
				WHEN MC.CostCenterId IS NOT NULL 
				THEN 'Mail Code Cost Center - '||MC.MailCode
				WHEN UPCASE(DD.DocName) LIKE ('%FED') 
					OR UPCASE(DD.DocName) LIKE ('%FED)') 
				THEN 'FED in Asset Name'
				ELSE BU.Unit
			END AS CostCenterDeterminant,
			CASE
				WHEN MC.CostCenterId IS NOT NULL 
				THEN MC.CostCenterId
				WHEN UPCASE(DD.DocName) LIKE ('%FED') 
					OR UPCASE(DD.DocName) LIKE ('%FED)') 
				THEN 13
				ELSE BC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN MC.CostCenterId IS NOT NULL 
				THEN MC.Weight 
				WHEN UPCASE(DD.DocName) LIKE ('%FED') 
					OR UPCASE(DD.DocName) LIKE ('%FED)') 
				THEN 100
				ELSE BC.Weight
			END AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN BSYS.LTDB_DAT_Requests SR
				ON TT.Lts = SR.Request
			INNER JOIN BSYS.LTDB_DAT_DocDetail DD
				ON SR.DocName = DD.DocName
			INNER JOIN BSYS.LTDB_DAT_CentralPrintingDocData CP
				ON DD.DocSeqNo = CP.DocSeqNo
			INNER JOIN BSYS.LTDB_REF_Unit BU
				ON DD.DocName = BU.DocName
			INNER JOIN CSYL.GENR_LST_BusinessUnits CU
				ON BU.Unit = CU.Name
			INNER JOIN CSYL.COST_DAT_BusinessUnitCostCenters BC
				ON CU.ID = BC.BusinessUnitId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN BC.EffectiveBegin AND COALESCE(BC.EffectiveEnd, DATE())
			INNER JOIN CSYL.COST_DAT_CostCenters BCC
				ON BC.CostCenterId = BCC.CostCenterId
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.COST_DAT_MailCodeCostCenters MC
				ON CP.UHEAACostCenter = MC.MailCode
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(MC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(MC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN MC.EffectiveBegin AND COALESCE(MC.EffectiveEnd, DATE())
	WHERE
			TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd	*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd	
		ORDER BY
			TT.TimeTrackingId
	;

/*	procedure requests - PMD*/
	CREATE TABLE Pmd AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Procedure Request' AS TaskType,
			TT.Pmd AS TaskNo,
			SR.ProcTitle AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			CASE
				WHEN UPCASE(SR.ProcTitle) LIKE ('%FED') 
					OR UPCASE(SR.ProcTitle) LIKE ('%FED)') 
				THEN 'FED in Asset Name'
				ELSE BU.Unit
			END AS CostCenterDeterminant,
			CASE
				WHEN UPCASE(SR.ProcTitle) LIKE ('%FED') 
					OR UPCASE(SR.ProcTitle) LIKE ('%FED)') 
				THEN 13 /*CornerStone Portfolio Servicing*/
				ELSE BC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN UPCASE(SR.ProcTitle) LIKE ('%FED') 
					OR UPCASE(SR.ProcTitle) LIKE ('%FED)') 
					OR BC.Weight IS NULL 
				THEN 100.0
				ELSE BC.Weight
			END AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN PMD.datRequests SR
				ON TT.Pmd = SR.Request
			INNER JOIN PMD.refUnit BU
				ON SR.ProcTitle = BU.ProcTitle
			INNER JOIN CSYL.GENR_LST_BusinessUnits CU
				ON BU.Unit = CU.Name
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.COST_DAT_BusinessUnitCostCenters BC
				ON CU.ID = BC.BusinessUnitId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BC.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN BC.EffectiveBegin AND COALESCE(BC.EffectiveEnd, DATE())
		WHERE
			TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	projects - Project*/ 
	CREATE TABLE Pj AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Project' AS TaskType,
			TT.Project AS TaskNo,
			PJ.pName AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'PJ Database' AS CostCenterDeterminant,
			PC.cCostCenterId AS CostCenterId,
			PC.cPercentAllocated AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN PJ.datProjects AS PJ
				ON TT.Project = PJ.pNo
			INNER JOIN PJ.refCostCenters PC
				ON TT.Project = PC.pNo
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd	*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	generic meetings*/
	CREATE TABLE Gm AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Generic Meeting' AS TaskType,
			. AS TaskNo,
			'' AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'Generic Meetings' AS CostCenterDeterminant,
			. AS CostCenterId,
			100 AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.Sr IS NULL
			AND TT.Sasr IS NULL
			AND TT.Lts IS NULL
			AND TT.Pmd IS NULL
			AND TT.Project IS NULL
			AND (TT.BatchScripts = '' OR TT.BatchScripts IS NULL)
			AND (TT.FsaCr = '' OR TT.FsaCr IS NULL)
			AND (TT.BillingScript = '' OR TT.BillingScript IS NULL)
			AND (TT.ConversionActivities = '' OR TT.ConversionActivities IS NULL)
			AND TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd	*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	old batch scripts - time spent running batch scripts should now be recorded using NH tickets, this is included for backward compatibility*/
	CREATE TABLE OldBatchScripts AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Batch Script' AS TaskType,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'Batch Scripts' AS CostCenterDeterminant,
			BS.CostCenterId AS CostCenterId,
			BS.Weight AS CostCenterWeight,
			CC.CostCenter AS CostCenter,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_BatchScriptWeights BS
/*				ON INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(BS.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(BS.EffectiveEnd, YYMMDD10.), DATE())*/
				ON TT.TaskDate BETWEEN BS.EffectiveBegin AND COALESCE(BS.EffectiveEnd, DATE())
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
			LEFT JOIN CSYL.COST_DAT_CostCenters CC
				ON BS.CostCenterId = CC.CostCenterId
		WHERE
			TT.BatchScripts ^= ''
			AND TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	FSA CR*/
	CREATE TABLE FsaCr AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'FSA CR' AS TaskType,
			. AS TaskNo,
			TT.FsaCr AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'FSA CR' AS CostCenterDeterminant,
			13 AS CostCenterId,
			100 AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.FsaCr ^= ''
			AND TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	billing script*/
	CREATE TABLE BillingScript AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Billing Script' AS TaskType,
			. AS TaskNo,
			TT.BillingScript AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'Billing Script' AS CostCenterDeterminant,
			CASE
				WHEN TT.BillingScript = 'U' THEN 8
				WHEN TT.BillingScript = 'C' THEN 13
				ELSE .
			END AS CostCenterId,
			100 AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.BillingScript ^= ''
			AND TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	converstion activities*/
	CREATE TABLE ConverstionActivities AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Conversion Activity' AS TaskType,
			. AS TaskNo,
			TT.ConversionActivities AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'Conversion Activities' AS CostCenterDeterminant,
			CASE
				WHEN TT.ConversionActivities = 'U' THEN 8
				WHEN TT.ConversionActivities = 'C' THEN 13
				ELSE .
			END AS CostCenterId,
			100 AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.ConversionActivities ^= ''
			AND TT.CostCenterId IS NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
	;

/*	cost center*/
	CREATE TABLE CostCenter AS
		SELECT DISTINCT
			'TT' AS RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			'Cost Center Specific' AS TaskType,
			. AS TaskNo,
			TT.CostCenter AS TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			'Cost Center' AS CostCenterDeterminant,
			TT.CostCenterId AS CostCenterId,
			100 AS CostCenterWeight,
			AW.Weight AS AgentWeight
		FROM
			CSY_TEST.COST_DAT_TimeTracking TT
			INNER JOIN CSY_TEST.COST_DAT_AgentWeights AW
				ON TT.SqlUserId = AW.SqlUserId
/*				AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN INPUT(AW.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(AW.EffectiveEnd, YYMMDD10.), DATE())*/
				AND TT.TaskDate BETWEEN AW.EffectiveBegin AND COALESCE(AW.EffectiveEnd, DATE())
		WHERE
			TT.CostCenterId IS NOT NULL
/*			AND INPUT(TT.TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			AND TT.TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
		ORDER BY
			TT.TimeTrackingId
;QUIT;

/*merge all tables to one data set*/
DATA TimeTrackingAllDetail;
	LENGTH 
		TaskType $ 30.
		TaskDescription $ 100.
		GenericMeetings $ 255.
	;
	SET NeedHelp 
		BatchScripts 
		Scr 
		Sas 
		Lts 
		Pmd 
		Pj 
		Gm 
		OldBatchScripts 
		FsaCr 
		BillingScript 
		ConverstionActivities 
		CostCenter 
;RUN;

/*add cost center name for user convenience*/
PROC SQL;
	CREATE TABLE AllDetail AS
		SELECT
			TT.RecordType,
			TT.TimeTrackingId,
			TT.TaskDate,
			TT.Hours,
			TT.TaskType,
			TT.TaskNo,
			TT.TaskDescription,
			TT.GenericMeetings,
			TT.Agent,
			TT.SqlUserId,
			TT.CostCenterDeterminant,
			TT.CostCenterId,
			TT.CostCenterWeight,
			CC.CostCenter,
			TT.AgentWeight
		FROM
			TimeTrackingAllDetail TT
			LEFT JOIN CSYL.COST_DAT_CostCenters CC
				ON TT.CostCenterId = CC.CostCenterId
;QUIT;

/*calculate weighted hours*/
PROC SQL;
	CREATE TABLE WeightedDetail AS
		SELECT
			D.RecordType,
			D.TimeTrackingId, 
			D.TaskDate,
			D.Hours,
			D.TaskType,
			D.TaskNo,
			D.TaskDescription,
			D.GenericMeetings,
			D.Agent,
			D.SqlUserId,
			D.CostCenterDeterminant,
			D.CostCenterId,
			D.CostCenterWeight,
			D.CostCenter,
			D.AgentWeight,
			S.TotalCostCenterWeight,
			D.CostCenterWeight/S.TotalCostCenterWeight AS WeightedWeight,
			D.CostCenterWeight/S.TotalCostCenterWeight*D.Hours AS WeightedHours,
			D.CostCenterWeight/S.TotalCostCenterWeight*D.Hours*D.AgentWeight AS AgentWeightedHours
		FROM
			AllDetail D
			INNER JOIN
			(
				SELECT
					RecordType,
					TimeTrackingId,
					SUM(CostCenterWeight) AS TotalCostCenterWeight
				FROM
					AllDetail
				GROUP BY
					RecordType,
					TimeTrackingId
			) S
				ON D.RecordType = S.RecordType
				AND D.TimeTrackingId = S.TimeTrackingId
		ORDER BY
			D.RecordType,
			D.TimeTrackingId
	;

	CREATE TABLE WeightedDetailOverhead AS
		SELECT
			*
		FROM
			WeightedDetail
		WHERE
			CostCenterId IS NULL
		ORDER BY
			CostCenterDeterminant
;QUIT;

/*calculate allocation percents*/
PROC SQL;
/*	all cost centers including overhead and non-billable cost centers*/
	CREATE TABLE AllocationsAll AS
		SELECT
			W.CostCenterId,
			CC.CostCenter,
			CC.IsBillable,
			CC.IsChargedOverHead,
			W.Hours,
			T.TotalHours,
			W.Hours/T.TotalHours*100 AS WeightedAllocationPercent,
			W.AgentHours,
			T.AgentTotalHours,
			W.AgentHours/T.AgentTotalHours*100 AS AgentWeightedAllocationPercent
		FROM
			(
				SELECT
					CostCenterId,
					SUM(AgentWeightedHours) AS AgentHours,
					SUM(WeightedHours) AS Hours
				FROM 
					WeightedDetail
				GROUP BY
					CostCenterId
			) W
			INNER JOIN 
			(
				SELECT
					SUM(AgentWeightedHours) AS AgentTotalHours,
					SUM(WeightedHours) AS TotalHours
				FROM
					WeightedDetail
			) T
			ON T.AgentTotalHours > 0
			LEFT JOIN CSYL.COST_DAT_CostCenters CC /*left join includes tasks with no cost center assigned (tasks charged to overhead)*/
				ON CC.CostCenterId = W.CostCenterId
;QUIT;

PROC SQL;
/*	break out overhead (hours not allocated to a cost center or to a non billable cost center) so it can be allocated to cost centers*/
	CREATE TABLE AllocationsOverHead AS
		SELECT
			SUM(Hours) AS Hours,
			SUM(AgentHours) AS AgentHours
		FROM
			AllocationsAll
		WHERE
			CostCenterId IS NULL
			OR IsBillable = 0
;QUIT;

PROC SQL;
/*	hours charged by agents to each cost center which is billable and charged a percent of overhead plus overhead hours allocated to the cost center*/
	CREATE TABLE AllocationsOfOverHead AS
		SELECT
			AP.CostCenterId,
			AP.CostCenter,
			AP.Hours,
			OH.Hours*AP.Hours/T.TotalHours AS OverHeadHours,
			AP.Hours+(OH.Hours*AP.Hours/T.TotalHours) AS TotalHours,
			AP.AgentHours,
			OH.AgentHours*AP.AgentHours/T.AgentTotalHours AS OverHeadAgentHours,
			AP.AgentHours+(OH.AgentHours*AP.AgentHours/T.AgentTotalHours) AS TotalAgentHours
		FROM
			AllocationsAll AP
			INNER JOIN /*totals for billable cost centers which are charged a percent of overhead*/
			(
				SELECT
					SUM(WD.AgentWeightedHours) AS AgentTotalHours,
					SUM(WD.WeightedHours) AS TotalHours
				FROM
					WeightedDetail WD
					LEFT JOIN CSYL.COST_DAT_CostCenters CC
						ON WD.CostCenterId = CC.CostCenterId
				WHERE
					WD.CostCenterId IS NOT NULL
					AND CC.IsBillable = 1
					AND CC.IsChargedOverHead = 1
			) T
				ON T.AgentTotalHours > 0
				AND AP.CostCenterId IS NOT NULL
				AND AP.IsBillable = 1
				AND AP.IsChargedOverHead = 1
			INNER JOIN AllocationsOverHead OH
				ON 1 = 1
;QUIT;

PROC SQL;
/*	hours for all cost centers with overhead allocated to individual cost centers*/
	CREATE TABLE AllocationsWithOverHead AS
		SELECT
			CostCenterId,
			CostCenter,
			Hours,
			AgentHours
		FROM
/*			cost centers with overhead hours allocated to them*/
			(
				SELECT
					CostCenterId,
					CostCenter,
					TotalHours AS Hours,
					TotalAgentHours AS AgentHours
				FROM
					AllocationsOfOverHead
			)
			UNION ALL
/*			cost centers which don't get overhead allocated to them*/
			(
				SELECT
					CostCenterId,
					CostCenter,
					Hours,
					AgentHours
				FROM
					AllocationsAll
				WHERE
					IsBillable = 1
					AND IsChargedOverHead = 0
			)
;QUIT;
			
PROC SQL;
/*	final allocations*/
	CREATE TABLE AllocationsFinal AS
		SELECT
			A.CostCenterId,
			A.CostCenter,
			A.Hours,
			T.TotalHours,
			A.Hours/T.TotalHours*100 AS AllocationPercent,
			A.AgentHours,
			T.AgentTotalHours,
			A.AgentHours/T.AgentTotalHours*100 AS AgentAllocationPercent
		FROM
/*			individual cost center records*/
			(
				SELECT
					CostCenterId,
					CostCenter,
					Hours,
					AgentHours
				FROM
					AllocationsWithOverHead
			) A
			INNER JOIN 
/*			totals needed to calculate percentages*/
			(
				SELECT
					SUM(Hours) AS TotalHours,
					SUM(AgentHours) AS AgentTotalHours
				FROM
					AllocationsWithOverHead
			) T
			ON T.AgentTotalHours > 0
;QUIT;

/*reports*/
PROC EXPORT 
	DATA = AllocationsFinal
    OUTFILE = "T:\SAS\Allocations.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Final";
RUN;

PROC EXPORT 
	DATA = AllocationsAll
    OUTFILE = "T:\SAS\Allocations.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Overhead";
RUN;

PROC EXPORT 
	DATA = WeightedDetail
    OUTFILE = "T:\SAS\Allocations Detail.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "All_Tasks";
RUN;

PROC EXPORT 
	DATA = WeightedDetailOverhead
    OUTFILE = "T:\SAS\Allocations Detail.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Overhead_Tasks";
RUN;

/*validation reports*/
PROC SQL;
/*	time tracking total hours*/
	CREATE TABLE VLD_TT_TH AS
		SELECT
			'TT' AS RecordType,
			SUM(Hours) AS TotalHours
		FROM 
			CSY_TEST.COST_DAT_TimeTracking
		WHERE
/*			INPUT(TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd	*/
			TaskDate BETWEEN &PeriodBegin AND &PeriodEnd	
	;

/*	Need Help total hours*/
	CREATE TABLE VLD_NH_TH AS
		SELECT
			'NH' AS RecordType,
			SUM((EndTime - StartTime)/3600) AS TotalHours
		FROM
			NeedHelpTimeTracking
	;

/*	total of weighted hours*/
	CREATE TABLE VLD_WH AS
		SELECT
			RecordType,
			SUM(WeightedHours) AS WeightedHours
		FROM
			WeightedDetail
		GROUP BY	
			RecordType
	;

/*	time tracking record count*/
	CREATE TABLE VLD_TT_TC AS
		SELECT
			'TT' AS RecordType,
			COUNT(TimeTrackingId) AS TotalCount
		FROM 
			CSY_TEST.COST_DAT_TimeTracking
		WHERE
/*			INPUT(TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
	;

/*	Need Help record count*/
	CREATE TABLE VLD_NH_TC AS
		SELECT
			'NH' AS RecordType,
			COUNT(TicketID) AS TotalCount
		FROM
			NeedHelpTimeTracking
	;

/*	count of weighted records*/
	CREATE TABLE VLD_WC AS
		SELECT
			RecordType,
			COUNT(DISTINCT TimeTrackingId) AS WeightedCount
		FROM 
			WeightedDetail
		GROUP BY 
			RecordType
;QUIT;

DATA ValidationCheckSums;
	MERGE 
		VLD_TT_TH 
		VLD_NH_TH 
		VLD_WH 
		VLD_TT_TC 
		VLD_NH_TC 
		VLD_WC;
	BY RecordType ;
RUN;
 
PROC SQL;
	CREATE TABLE MissingNH AS
		SELECT 
			TT.*
		FROM	
			NeedHelpTimeTracking TT 
			LEFT JOIN 
			(
				SELECT
					*
				FROM
					NeedHelp

				UNION

				SELECT
					*
				FROM
					BatchScripts
			) NH
				ON NH.TimeTrackingId = TT.TimeTrackingId
		WHERE
			NH.TimeTrackingId IS NULL
	;

	CREATE TABLE TimeTracking AS
		SELECT
			*
		FROM 
			CSY_TEST.COST_DAT_TimeTracking
		WHERE
/*			INPUT(TaskDate, YYMMDD10.) BETWEEN &PeriodBegin AND &PeriodEnd*/
			TaskDate BETWEEN &PeriodBegin AND &PeriodEnd
	;

	CREATE TABLE Detail AS
		SELECT DISTINCT
			TimeTrackingId	
		FROM
			AllDetail 
		WHERE
			RecordType = 'TT'
	;

	CREATE TABLE MissingTT AS
		SELECT
			TT.*
		FROM
			TimeTracking TT
			LEFT JOIN Detail DT
				ON TT.TimeTrackingId = DT.TimeTrackingId
		WHERE
			DT.TimeTrackingId IS NULL
		ORDER BY
			TT.TimeTrackingId
;QUIT;

PROC EXPORT 
	DATA = ValidationCheckSums
    OUTFILE = "T:\SAS\Allocations Validation.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Check_Sums";
RUN;

/*records in Need Help (Reporting.TimeTracking) which are missing from the final results*/
PROC EXPORT 
	DATA = MissingNH
    OUTFILE = "T:\SAS\Allocations Validation.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Need_Help";
RUN;

/*records from Excel tracking spreadsheets which are missing from the final results*/
PROC EXPORT 
	DATA = MissingTT
    OUTFILE = "T:\SAS\Allocations Validation.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Excel";
RUN;

LIBNAME _ALL_ CLEAR;
