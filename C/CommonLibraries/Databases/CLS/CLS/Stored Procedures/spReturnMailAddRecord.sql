CREATE PROCEDURE spReturnMailAddRecord
	-- Add the parameters for the stored procedure here
	@UserId			VARCHAR(50),
	@RecipientId	VARCHAR(10),
	@LetterId		VARCHAR(10),
	@CreateDate		DATETIME,
	@PersonType		CHAR(2),
	@Address1		VARCHAR(30) = NULL,
	@Address2		VARCHAR(30) = NULL,
	@City			VARCHAR(20) = NULL,
	@State			CHAR(2) = NULL,
	@Zip			VARCHAR(9) = NULL,
	@Country		VARCHAR(25) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ReturnMail (
		UserId,
		RecipientId,
		LetterId,
		CreateDate,
		PersonType,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Country
	)
	VALUES (
		@UserId,
		@RecipientId,
		@LetterId,
		@CreateDate,
		@PersonType,
		@Address1,
		@Address2,
		@City,
		@State,
		@Zip,
		@Country
	)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spReturnMailAddRecord] TO [db_executor]
    AS [dbo];



