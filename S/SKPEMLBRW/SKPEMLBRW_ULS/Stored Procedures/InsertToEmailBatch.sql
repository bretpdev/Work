CREATE PROCEDURE [skpemlbrw].[InsertToEmailBatch]
	@EmailCampaignId int,
	@AccountNumber char(10),
	@EmailData varchar(max),
	@ArcNeeded bit
AS
	DECLARE @Now DATETIME = GETDATE()
	DECLARE @User VARCHAR(50) = SUSER_NAME()

	INSERT INTO emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, EmailData, ArcNeeded, AddedBy, AddedAt)
	SELECT DISTINCT
		EC.EmailCampaignId,
		@AccountNumber,
		@EmailData,
		@ArcNeeded,
		@User,
		@Now
	FROM
		emailbatch.EmailCampaigns EC
		LEFT JOIN emailbatch.EmailProcessing EP
			ON EC.EmailCampaignId = EP.EmailCampaignId
			AND EP.AccountNumber = @AccountNumber
			AND EP.EmailData = @EmailData
			AND EP.ArcNeeded = @ArcNeeded
			AND EP.EmailSentAt IS NULL
			AND EP.DeletedAt IS NULL
			AND EP.DeletedBy IS NULL
	WHERE
		EC.EmailCampaignId = @EmailCampaignId
		AND EP.EmailProcessingId IS NULL

	SELECT SCOPE_IDENTITY()
