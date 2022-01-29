CREATE PROCEDURE [rtnemlinvf].[InsertInvalidEmail]
	@Ssn CHAR(9),
	@EmailType CHAR(1),
	@ReceivedBy VARCHAR(254),
	@Subject VARCHAR(1000),
	@ReceivedDate DATETIME
AS

	DECLARE @SsnFound CHAR(9) = 
	(
		SELECT
			Ssn
		FROM
			rtnemlinvf.InvalidEmail
		WHERE
			ReceivedBy = @ReceivedBy
			AND CAST(ReceivedDate AS DATE) = CAST(@ReceivedDate AS DATE)
			AND Ssn = @Ssn
			AND EmailType = @EmailType
	)
	
	IF @SsnFound = '' OR @SsnFound IS NULL
	BEGIN
		INSERT INTO rtnemlinvf.InvalidEmail(Ssn, EmailType, [Subject], ReceivedBy, ReceivedDate)
		VALUES(@Ssn, @EmailType, @Subject, @ReceivedBy, @ReceivedDate)
	END
RETURN 0