CREATE PROCEDURE [dbo].[DTX7LInsertDeletedRecord]
	@Ssn CHAR(9),
	@Arc VARCHAR(5),
	@RequestDate DATETIME,
	@LetterId VARCHAR(10),
	@IsDueDiligence BIT
AS
	INSERT INTO DTX7LDeletedRecords(Ssn,Arc,RequestDate,LetterId, IsDueDiligence)
	VALUES(@Ssn, @Arc, @RequestDate, @LetterId, @IsDueDiligence)
RETURN 0
