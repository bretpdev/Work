CREATE PROCEDURE [dbo].[spFLOW_GetFlowsForSpecifiedSystem] 
	@System					VARCHAR(30)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * 
	FROM dbo.FLOW_dat_Flow
	WHERE System = @System
    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetFlowsForSpecifiedSystem] TO [db_executor]
    AS [dbo];

