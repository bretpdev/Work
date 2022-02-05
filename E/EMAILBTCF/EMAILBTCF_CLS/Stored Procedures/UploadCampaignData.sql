CREATE PROCEDURE [emailbtcf].[UploadCampaignData]
	@EmailCampaignId INT,
	@CampaignData BorrowerInfo READONLY
AS
	
	INSERT INTO
		emailbtcf.CampaignData (EmailCampaignId, Recipient, AccountNumber, FirstName, LastName)
	SELECT
		@EmailCampaignId, Recipient, AccountNumber, FirstName, LastName
	FROM
		@CampaignData

RETURN 0
