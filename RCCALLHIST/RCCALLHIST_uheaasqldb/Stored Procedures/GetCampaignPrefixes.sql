CREATE PROCEDURE [rccallhist].[GetCampaignPrefixes]
AS
	DECLARE @ScriptId VARCHAR(20) = 'RCCALLHIST'

	SELECT 
		CampaignPrefix
	FROM
		CampaignPrefixes
	WHERE
		ScriptId = @ScriptId
RETURN 0
