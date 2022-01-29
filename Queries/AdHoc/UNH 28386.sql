USE [NobleCalls]
GO
/****** Object:  StoredProcedure [dbo].[GetCallsForRegion]    Script Date: 8/3/2016 3:08:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetCallsForRegion]
	@Region VARCHAR(20)
AS
	
	SELECT
		NCH.NobleCallHistoryId,
		NCH.AccountIdentifier,
		NCH.CallType,
		NCH.CallCampaign,
		NCH.ActivityDate,
		NCH.PhoneNumber,
		NCH.AgentId,
		NCH.DispositionCode,
		NCH.AdditionalDispositionCode,
		NCH.RegionId
	FROM
		NobleCallHistory NCH
		INNER JOIN Regions R
			ON R.RegionId = NCH.RegionId
		WHERE
			R.Region = @Region
			AND NCH.Deleted = 0
			AND NCH.ArcAddProcessingId IS NULL
			AND NCH.IsInbound = 0
			
RETURN 0
GO

UPDATE
	NobleCallHistory
SET
	Deleted = 1,
	DeletedBy = SUSER_SNAME(),
	DeletedAt = GETDATE()
WHERE
	NobleCallHistoryId IN
		(SELECT NobleCallHistoryId FROM NobleCallHistory WHERE IsInbound = 1 AND AccountIdentifier IS NOT NULL AND IsReconciled = 0)