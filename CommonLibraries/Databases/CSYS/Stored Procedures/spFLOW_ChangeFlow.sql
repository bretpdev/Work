CREATE PROCEDURE [dbo].[spFLOW_ChangeFlow]
	@FlowID								VARCHAR(50),
	@System								VARCHAR(30),
	@Description						VARCHAR(8000),
	@ControlDisplayText					VARCHAR(200),
	@UserInterfaceDisplayIndicator		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE dbo.FLOW_dat_Flow
	SET System = @System,
		Description = @Description,
		ControlDisplayText = @ControlDisplayText,
		UserInterfaceDisplayIndicator = @UserInterfaceDisplayIndicator
	WHERE FlowID = @FlowID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_ChangeFlow] TO [db_executor]
    AS [dbo];


