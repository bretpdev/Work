CREATE PROCEDURE [dbo].[GetCampaignPrefixes]
	@ScriptId VARCHAR(100)
AS
	SELECT DISTINCT
		CP.CampaignPrefix
	FROM
		CampaignPrefixes CP
	WHERE
		CP.Active = 1
		AND CP.ScriptId = @ScriptId
RETURN 0
