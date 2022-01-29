
	CREATE PROCEDURE [dbo].[GetCallCampaigns]
		@Region VARCHAR(20)
	AS
		SELECT 
			C.CallCampaign
		FROM 
			CallCampaigns C 
			INNER JOIN Regions R 
				ON R.RegionId = C.RegionId
		WHERE
			R.Region = @Region
			AND R.Region != 'EXCLUDE'
RETURN 0
