CREATE PROCEDURE [emailbtcf].[UploadCampaignDataSingle]
	@EmailCampaignId INT,
	@Recipient VARCHAR(254),
	@AccountNumber VARCHAR(10),
	@FirstName VARCHAR(100),
	@LastName VARCHAR(100)
AS
	
	INSERT INTO
		emailbtcf.CampaignData (EmailCampaignId, Recipient, AccountNumber, FirstName, LastName)
	SELECT
		@EmailCampaignId, @Recipient, @AccountNumber, @FirstName, @LastName
