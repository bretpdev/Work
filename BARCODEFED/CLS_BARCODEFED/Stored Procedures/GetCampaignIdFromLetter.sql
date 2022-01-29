CREATE PROCEDURE [barcodefed].[GetCampaignIdFromLetter]
	@LetterId varchar(50),
	@IsEndorser bit
AS
	SELECT
		EmailCampaignId
	FROM
		emailbtcf.EmailCampaigns
	WHERE
		LetterId = @LetterId
		AND
		(
			(@IsEndorser = 1 AND ARC = 'CAEEM')
			OR
			(@IsEndorser = 0 AND ARC = 'CABEM')
		)
RETURN 0