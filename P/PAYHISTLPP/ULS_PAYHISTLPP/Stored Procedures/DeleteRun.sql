CREATE PROCEDURE [payhistlpp].[DeleteRun]
	@RunId INT
AS
	UPDATE
		ULS.payhistlpp.Run
	SET
		DeletedAt = GETDATE(),
		DeletedBy = USER_NAME()
	WHERE
		RunId = @RunId