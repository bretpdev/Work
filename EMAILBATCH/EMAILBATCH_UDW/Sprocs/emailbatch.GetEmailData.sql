CREATE PROCEDURE [emailbatch].[GetEmailData]

AS

BEGIN TRANSACTION 
DECLARE @ERROR INT = 0

DECLARE @Emails as table
(
EmailProcessingId INT
)

SET @ERROR = @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED CREATING THE @EMAILS TABLE', 16, 1) 
END

INSERT INTO @Emails
	SELECT 
		EP.EmailProcessingId
	FROM
		[emailbatch].EmailProcessing EP
	WHERE
		EP.DeletedAt IS NULL
		AND (EP.EmailSentAt IS NULL OR EP.ArcAddProcessingId IS NULL)
		AND EP.ProcessingAttempts < 2

SET @ERROR = @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED INSERTING DATA INTO THE @EMAILS TABLE', 16, 1) 
END

UPDATE 
	EP
SET
	ProcessingAttempts = (EP.ProcessingAttempts + 1)
from
	[emailbatch].EmailProcessing EP
	INNER JOIN @Emails E
		ON E.EmailProcessingId = EP.EmailProcessingId

SET @ERROR = @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED UPDATING THE PROCESSING ATTEMPTS', 16, 1) 
END

	SELECT 
		EP.EmailProcessingId,
		EP.EmailCampaignId,
		EP.AccountNumber,
		EP.EmailData,
		EP.EmailSentAt,
		EP.ArcNeeded,
		EP.ArcAddProcessingId,
		HF.HTMLFile,
		FA.FromAddress,
		SL.SubjectLine,
		A.Arc,
		C.Comment,
		ACT.ActivityType,
		AC.ActivityContact
	FROM
		[emailbatch].EmailProcessing EP
		INNER JOIN @Emails E
			ON E.EmailProcessingId = EP.EmailProcessingId
		INNER JOIN [emailbatch].EmailCampaigns EC
			ON EC.EmailCampaignId = EP.EmailCampaignId
		INNER JOIN [emailbatch].HTMLFiles HF
			ON HF.HTMLFileId = EC.HTMLFileId
		INNER JOIN [emailbatch].FromAddresses FA
			ON FA.FromAddressId = EC.FromAddressId
		INNER JOIN [emailbatch].SubjectLines SL
			ON SL.SubjectLineId = EC.SubjectLineId
		LEFT JOIN [emailbatch].Arcs A
			ON A.ArcId = EC.ArcId
		LEFT JOIN [emailbatch].Comments C
			ON C.CommentId = EC.CommentId
		LEFT JOIN emailbatch.ActivityTypes ACT
			ON ACT.ActivityTypeId = EC.ActivityTypeId
		LEFT JOIN emailbatch.ActivityContacts AC
			ON AC.ActivityContactId = EC.ActivityContactId

SET @ERROR = @@ERROR

IF @ERROR != 0
BEGIN
	ROLLBACK
	RAISERROR('AN ERROR OCCURED PULLING THE ACCOUNTS TO PROCESS', 16, 1) 
END
ELSE
BEGIN
	COMMIT
END

			
RETURN 0
