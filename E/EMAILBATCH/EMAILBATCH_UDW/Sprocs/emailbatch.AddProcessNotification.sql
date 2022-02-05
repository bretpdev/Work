CREATE PROCEDURE [emailbatch].[AddProcessNotification]
	@EmailProcessingId INT,
	@ProcessNotificationId INT
AS
	INSERT INTO EmailProcessLogMapping
	VALUES(@EmailProcessingId, @ProcessNotificationId)
RETURN 0
