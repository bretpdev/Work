CREATE PROCEDURE ResetEstimates

AS

UPDATE bsys.dbo.SCKR_DAT_SASRequests SET DevEstimateBegin = NULL, DevEstimateEnd = NULL, TestEstimateBegin = NULL, TestEstimateEnd = NULL
WHERE CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')

UPDATE bsys.dbo.SCKR_DAT_ScriptRequests SET DevEstimateBegin = NULL, DevEstimateEnd = NULL, TestEstimateBegin = NULL, TestEstimateEnd = NULL
WHERE CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')

UPDATE bsys.dbo.LTDB_DAT_Requests SET SetupEstimateBegin = NULL, SetupEstimateEnd = NULL, TestEstimateBegin = NULL, TestEstimateEnd = NULL
WHERE CurrentStatus NOT IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')


