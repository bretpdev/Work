CREATE PROCEDURE spReturnMailDeleteRecord
	-- Add the parameters for the stored procedure here
	@UserId			VARCHAR(50),
	@RecipientId	VARCHAR(10),
	@LetterId		VARCHAR(10),
	@CreateDate		DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM ReturnMail
	WHERE UserId = @UserId
		AND RecipientId = @RecipientId
		AND LetterId = @LetterId
		AND CreateDate = @CreateDate
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spReturnMailDeleteRecord] TO [db_executor]
    AS [dbo];



