CREATE PROCEDURE spDocIdSetProcessed
	@UserName	VARCHAR(50),
	@DocId		VARCHAR(5),
	@Source		VARCHAR(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO DocIdProcessed (
		DateTimeStamp,
		UserName,
		DocId,
		[Source]
	)
	VALUES (
		GETDATE(),
		@UserName,
		@DocId,
		@Source
	)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDocIdSetProcessed] TO [db_executor]
    AS [dbo];



