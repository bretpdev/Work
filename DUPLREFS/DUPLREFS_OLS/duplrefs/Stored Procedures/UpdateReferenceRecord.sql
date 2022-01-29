CREATE PROCEDURE [duplrefs].[UpdateReferenceRecord]
	@ReferenceQueueId INT,
	@RefId VARCHAR(9),
	@RefName VARCHAR(50),
	@RefAddress1 VARCHAR(35),
	@RefAddress2 VARCHAR(35),
	@RefCity VARCHAR(30),
	@RefState VARCHAR(2),
	@RefZip VARCHAR(14),
	@RefCountry VARCHAR(25),
	@RefPhone VARCHAR(21),
	@RefStatus CHAR(1),
	@ValidAddress BIT,
	@ValidPhone BIT,
	@DemosChanged BIT,
	@ZipChanged BIT,
	@Duplicate BIT,
	@PossibleDuplicate BIT

AS

	UPDATE
		RQ
	SET
		RQ.RefAddress1 = @RefAddress1,
		RQ.RefAddress2 = @RefAddress2,
		RQ.RefCity = @RefCity,
		RQ.RefState = @RefState,
		RQ.RefZip = @RefZip,
		RQ.RefCountry = @RefCountry,
		RQ.RefPhone = @RefPhone,
		RQ.RefStatus = @RefStatus,
		RQ.ValidAddress = @ValidAddress,
		RQ.ValidPhone = @ValidPhone,
		RQ.DemosChanged = @DemosChanged,
		RQ.ZipChanged = @ZipChanged
	FROM	
		[duplrefs].ReferenceQueue RQ
	WHERE
		RQ.ReferenceQueueId = @ReferenceQueueId
           
RETURN 0
