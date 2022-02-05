CREATE PROCEDURE [dbo].[InsertFutureAltFormatLetter]
	@AccountNumber CHAR(10),
	@LetterId VARCHAR(10),
	@AddedBy VARCHAR(50),
	@CorrespondenceFormatId int
AS
	
	INSERT INTO FutureDatedAltFormatRequests(AccountNumber, LetterId, AddedBy, CorrespondenceFormatId)
	VALUES(@AccountNumber, @LetterId, @AddedBy, @CorrespondenceFormatId)
RETURN 0
