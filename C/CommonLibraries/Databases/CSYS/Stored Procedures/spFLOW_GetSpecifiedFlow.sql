
CREATE PROCEDURE [dbo].[spFLOW_GetSpecifiedFlow]
	@FlowID						VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM dbo.FLOW_Dat_Flow WHERE FlowID = @FlowID
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetSpecifiedFlow] TO [db_executor]
    AS [dbo];


