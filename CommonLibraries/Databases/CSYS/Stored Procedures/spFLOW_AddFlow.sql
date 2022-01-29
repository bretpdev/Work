
CREATE PROCEDURE [dbo].[spFLOW_AddFlow]
	@FlowID						VARCHAR(50),
	@Description				VARCHAR(8000),
	@ControlDisplayText			VARCHAR(200),
	@UIDisplayIndicator			VARCHAR(50),
	@System						VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO dbo.FLOW_DAT_Flow
	(FlowID, Description, ControlDisplayText, UserInterfaceDisplayIndicator, System) 
	VALUES 
	(@FlowID, @Description, @ControlDisplayText, @UIDisplayIndicator, @System)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_AddFlow] TO [db_executor]
    AS [dbo];

