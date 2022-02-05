CREATE PROCEDURE [emailbatch].[LoadEmailData]
	@EmailCampaignId INT,
	@SourceFile VARCHAR(300),
	@AddedBy varchar(50)
AS
	
BEGIN TRANSACTION 
DECLARE @ERROR INT = 0 

INSERT INTO [emailbatch].EmailProcessing
(
	EmailCampaignId,
	AccountNumber,
	ActualFile,
	EmailData,
	ArcNeeded,
	AddedBy,
	AddedAt
)
SELECT
	@EmailCampaignId,
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', 0, 1) as AccountNumber, 
	@SourceFile,
	BL.LineData,
	CASE WHEN EC.ArcId IS NULL THEN 0 ELSE 1 END,
	@AddedBy,
	GETDATE()
FROM
	[emailbatch]._BulkLoad BL
	INNER JOIN [emailbatch].EmailCampaigns EC
		ON EC.EmailCampaignId = @EmailCampaignId


SET @ERROR = @@ERROR

DELETE _BulkLoad


SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED INSERTING DATA FOR EMAILCAMPAIGNID:%d', 16, 1, @EmailCampaignId) 
END
ELSE
	COMMIT

RETURN 0
