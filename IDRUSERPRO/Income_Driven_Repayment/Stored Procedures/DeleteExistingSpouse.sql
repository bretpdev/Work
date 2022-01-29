CREATE PROCEDURE [dbo].[DeleteExistingSpouse]
	@SpouseId INT
AS
BEGIN
	BEGIN TRANSACTION
		DECLARE @ROWCOUNT INT = 0,
		@ERROR INT = 0

		UPDATE
			dbo.Applications
		SET spouse_id = NULL
		WHERE spouse_id = @SpouseId

		SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
		DELETE 
		FROM
			 dbo.Spouses
		WHERE 
			spouse_id = @SpouseId

		SELECT @ROWCOUNT = @@ROWCOUNT + @ROWCOUNT, @ERROR = @@ERROR + @ERROR

		IF @ROWCOUNT = 2 AND @ERROR = 0
			BEGIN
				COMMIT TRANSACTION
			END
		ELSE
			BEGIN
				ROLLBACK TRANSACTION
			END
END