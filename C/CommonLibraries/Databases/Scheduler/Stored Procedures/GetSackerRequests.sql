CREATE PROCEDURE GetSackerRequests

AS

SELECT 
RT.RequestType AS RequestType, 
LT.Request AS RequestId,
LT.SetupEstimateBegin AS DevStartDate,
LT.SetupEstimateEnd AS DevEndDate,
ISNULL(LT.SetupEstimateComplete, DRE.DevEstimate)  AS DevEstimate,
LT.TestEstimateBegin AS TesterStartDate,
LT.TestEstimateEnd AS TesterEndDate,
ISNULL(LT.TestEstimateComplete, DRE.TestEstimate) AS TesterEstimate,
LT.SOC AS AssignedDeveloper,
LT.Tester AS AssignedTester,
LT.CurrentStatus AS CurrentStatus
FROM [BSYS].[dbo].[LTDB_DAT_Requests] LT 
	INNER JOIN RequestTypes RT ON RT.RequestType = 'Letter'
	INNER JOIN DefaultRequestEstimates DRE ON RT.RequestTypeId = DRE.RequestTypeId 
WHERE LT.CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')

UNION ALL

SELECT 
RT.RequestType AS RequestType,
SR.Request AS RequestId,
SR.DevEstimateBegin AS DevStartDate,
SR.DevEstimateEnd AS DevEndDate,
ISNULL(SR.DevEstimateComplete, DRE.DevEstimate) AS DevEstimate,
SR.TestEstimateBegin AS TesterStartDate,
SR.TestEstimateEnd AS TesterEndDate,
ISNULL(SR.TestEstimateComplete, DRE.TestEstimate) AS TesterEstimate,
RP.Programmer AS AssignedDeveloper,
SR.SSA AS AssignedTester,
SR.CurrentStatus AS CurrentStatus
FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests] SR
	LEFT OUTER JOIN [BSYS].[dbo].[SCKR_REF_Programmer] RP 
		ON SR.Request = RP.Request AND RP.[End] IS NULL AND RP.[Begin] = (SELECT MAX([Begin]) from BSYS.dbo.SCKR_REF_Programmer where Request = RP.Request) AND RP.Class = 'Scr'
	INNER JOIN RequestTypes RT ON RT.RequestType = 'Script'
	INNER JOIN DefaultRequestEstimates DRE ON RT.RequestTypeId = DRE.RequestTypeId
WHERE SR.CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')

UNION ALL

SELECT 
RT.RequestType AS RequestType,
SASR.Request AS RequestId,
SASR.DevEstimateBegin AS DevStartDate,
SASR.DevEstimateEnd AS DevEndDate,
isnull(SASR.DevEstimateComplete, DRE.DevEstimate) AS DevEstimate,
SASR.TestEstimateBegin AS TesterStartDate,
SASR.TestEstimateEnd AS TesterEndDate,
isnull(SASR.TestEstimateComplete, DRE.TestEstimate) AS TesterEstimate,
RP.Programmer AS AssignedDeveloper,
SASR.SSA AS AssignedTester,
SASR.CurrentStatus AS CurrentStatus
FROM [BSYS].[dbo].[SCKR_DAT_SASRequests] SASR
	LEFT OUTER JOIN [BSYS].[dbo].[SCKR_REF_Programmer] RP 
		ON SASR.Request = RP.Request AND RP.[End] IS NULL AND RP.[Begin] = (SELECT MAX([Begin]) from BSYS.dbo.SCKR_REF_Programmer where Request = RP.Request) AND RP.Class = 'SAS'
	INNER JOIN RequestTypes RT ON RT.RequestType = 'SAS'
	INNER JOIN DefaultRequestEstimates DRE ON RT.RequestTypeId = DRE.RequestTypeId 
WHERE SASR.CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')


