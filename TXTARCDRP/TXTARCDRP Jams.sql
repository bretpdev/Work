BEGIN TRANSACTION
DECLARE @ArcTracker TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10));
DECLARE @Arc CHAR(5) = 'BRTXT';
DECLARE @ScriptId VARCHAR(10) = 'TXTARCDRP';
DECLARE @Today DATE = GETDATE();
DECLARE @Now DATETIME = GETDATE();

INSERT INTO ULS..ArcAddProcessing(ArcTypeId, ArcResponseCodeId, AccountNumber, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcTracker(ArcAddProcessingId, AccountNumber)
SELECT
	1 AS ArcTypeId,
	NULL AS ArcResponseCodeId,
	TD.AccountNumber AS AccountNumber,
	NULL AS RecipientId,
	@Arc AS ARC,
	@ScriptId AS ScriptId,
	@Now AS ProcessOn,
	'Text message sent to ' + Texts.friendly_to + ' in regards to ' + TD.ContentType AS Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	txt.dbo.twilio_data Texts --Table storing text messages sent by the BU
	INNER JOIN ULS.textcoord.TrackingData TD --Table storing data about the file we sent to Twilio
		ON TD.PhoneNumber = Texts.friendly_to
		AND Texts.txt_created BETWEEN TD.CreatedAt AND DATEADD(DAY,7,TD.CreatedAt)
	LEFT JOIN ULS..ArcAddProcessing AAP
		ON AAP.AccountNumber = TD.AccountNumber
		AND AAP.ARC = @Arc
		AND AAP.ScriptId = @ScriptId
		AND AAP.Comment = 'Text message sent to ' + Texts.friendly_to + ' in regards to ' + TD.ContentType
		AND CAST(AAP.CreatedAt AS DATE) = @Today
WHERE
	Texts.[status] = 'delivered' --Texts can be undelivered and we dont want to arc those ones
	AND TD.ArcAddProcessingId IS NULL --Find only records where we havent already added to arc add
	AND TD.DeletedAt IS NULL
	AND TD.DeletedBy IS NULL
	AND AAP.AccountNumber IS NULL;

--After adding arcs into arcadd, update the tracking database to show the arc add id
UPDATE 
	TD
SET
	TD.ArcAddProcessingId = ArcT.ArcAddProcessingId
FROM
	@ArcTracker ArcT
	INNER JOIN ULS.textcoord.TrackingData TD
		ON ArcT.AccountNumber = TD.AccountNumber
WHERE
	TD.ArcAddProcessingId IS NULL
	AND TD.DeletedAt IS NULL
	AND TD.DeletedBy IS NULL;
COMMIT TRANSACTION