CREATE PROCEDURE [dbo].[spFLOW_CheckForExistenceOfFlow]
	@FlowID						VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT COUNT(*) FROM dbo.FLOW_dat_Flow WHERE FlowID = @FlowID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_CheckForExistenceOfFlow] TO [db_executor]
    AS [dbo];


GO
