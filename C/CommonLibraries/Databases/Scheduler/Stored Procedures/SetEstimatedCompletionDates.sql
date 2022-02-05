CREATE PROCEDURE SetEstimatedCompletionDates
(
	@DevStartDate datetime,
	@DevEndDate datetime,
	@TestStartDate datetime,
	@TestEndDate datetime,
	@Request int,
	@RequestType varchar(6)
)
AS

if(@RequestType = 'Letter')
begin
update LT set 
LT.SetupEstimateBegin = @DevStartDate,
LT.SetupEstimateEnd = @DevEndDate,
LT.TestEstimateBegin = @TestStartDate,
LT.TestEstimateEnd = @TestEndDate
FROM [BSYS].[dbo].[LTDB_DAT_Requests] LT 
where LT.Request = @Request
end

if(@RequestType = 'Script')
begin
update SR set 
SR.DevEstimateBegin = @DevStartDate,
SR.DevEstimateEnd = @DevEndDate,
SR.TestEstimateBegin = @TestStartDate,
SR.TestEstimateEnd = @TestEndDate
FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests] SR 
where SR.Request = @Request
end

if(@RequestType = 'SAS')
begin
update SASR set 
SASR.DevEstimateBegin = @DevStartDate,
SASR.DevEstimateEnd = @DevEndDate,
SASR.TestEstimateBegin = @TestStartDate,
SASR.TestEstimateEnd = @TestEndDate
FROM [BSYS].[dbo].[SCKR_DAT_SASRequests] SASR 
where SASR.Request = @Request
end


