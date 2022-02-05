CREATE PROCEDURE [rtrnmailol].[SaveScannedInfo]
	@RecipientId VARCHAR(10),
	@LetterId VARCHAR(10),
	@CreateDate DATETIME,
	@ReceivedDate DATETIME,
	@Address1 VARCHAR(50) = NULL,
	@Address2 VARCHAR(50) = NULL,
	@City VARCHAR(50) = NULL,
	@State VARCHAR(50) = NULL,
	@ZipCode VARCHAR(50) = NULL,
	@Comment VARCHAR(360) = NULL
AS
	
	INSERT INTO	[rtrnmailol].BarcodeData(AccountIdentifier, LetterId, CreateDate, ReceivedDate, Address1, Address2, City, [State], Zip, Comment)
	VALUES(@RecipientId, @LetterId, @CreateDate, @ReceivedDate, @Address1, @Address2, @City, @State, @ZipCode, @Comment)

SELECT SCOPE_IDENTITY()