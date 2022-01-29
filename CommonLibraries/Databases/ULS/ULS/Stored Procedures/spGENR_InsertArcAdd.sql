
CREATE PROCEDURE [dbo].[spGENR_InsertArcAdd]
	-- Add the parameters for the stored procedure here
	@RequestedDate		datetime,
	@AccountNumber		char(10),
	@RecipientId		varchar(9),
	@ARC				varchar(5),
	@QueueDueDate		datetime,
	@Comment			varchar(1200),
	@UserId				varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.GENR_DAT_ArcAdd (RequestedDate, AccountNumber, RecipientId, ARC, QueueDueDate, Comment, UserId) 
	VALUES (@RequestedDate, @AccountNumber, @RecipientId, @ARC, @QueueDueDate, @Comment, @UserId)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_InsertArcAdd] TO [db_executor]
    AS [dbo];

