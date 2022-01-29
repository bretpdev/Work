CREATE PROCEDURE [phonesucsn].[SetProcessedAt]
	@PhoneSuccessionId INT
AS
	UPDATE
		phonesucsn.PhoneSuccession
	SET
		ProcessedAt = GETDATE()
	WHERE
		PhoneSuccessionId = @PhoneSuccessionId