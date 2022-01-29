CREATE PROCEDURE [NobleController].[GetSprocForCallingCampaign]
	@Campaign VARCHAR(200)
AS
	SELECT
		StoredProcedureName
	FROM
		[NobleController].CallCampaignSprocMap
	WHERE
		Campaign = @Campaign
