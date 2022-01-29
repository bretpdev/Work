CREATE PROCEDURE spReturnMailGetRecords
	-- Add the parameters for the stored procedure here
	@UserId			VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
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
	FROM ReturnMail
	WHERE UserId = @UserId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spReturnMailGetRecords] TO [db_executor]
    AS [dbo];



