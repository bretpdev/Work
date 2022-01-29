CREATE PROCEDURE spCentralizedPrintingFinalUpdateFaxRecord
	@RightFaxHandle VARCHAR(50),
	@FinalStatus	VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingFax
	SET Confirmed = GETDATE(),
		FinalStatus = @FinalStatus 
	WHERE RightFaxHandle = @RightFaxHandle
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingFinalUpdateFaxRecord] TO [db_executor]
    AS [dbo];



