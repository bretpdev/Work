CREATE PROCEDURE [dbo].[ProcessNotificationInsert]
	@NotificationTypeId int,
	@NotificationSeverityTypeId int,
	@ScriptLogId int,
	@ExceptionLogId int = null
AS

BEGIN
	SET NOCOUNT ON

	INSERT INTO ProcessNotifications(NotificationTypeId, NotificationSeverityTypeId, ProcessLogId)
	VALUES(@NotificationTypeId, @NotificationSeverityTypeId, @ScriptLogId)

	SET NOCOUNT OFF
	SELECT CAST(SCOPE_IDENTITY() AS int)
END