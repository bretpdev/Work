
CREATE PROCEDURE [dbo].[GetCallsForRegion]
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
