CREATE PROCEDURE spCheckByPhoneUpdateProcessedDate
	@RecordNumber	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CheckByPhone
	SET ProcessedDate = GETDATE()
	WHERE RecNo = @RecordNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckByPhoneUpdateProcessedDate] TO [db_executor]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckByPhoneUpdateProcessedDate] TO [UHEAA\BatchScripts]
    AS [dbo];



