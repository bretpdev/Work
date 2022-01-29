CREATE PROCEDURE [payhistlpp].[UpdateRunDir]
	@Dir VARCHAR(100),
	@RunId INT
AS
	UPDATE
		payhistlpp.Run
	SET
		FileDirectory = @Dir
	WHERE
		RunId = @RunId