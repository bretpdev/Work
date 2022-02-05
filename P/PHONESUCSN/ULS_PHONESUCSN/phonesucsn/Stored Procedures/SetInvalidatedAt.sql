CREATE PROCEDURE [phonesucsn].[SetInvalidatedAt]
	@PhoneSuccessionid INT
AS
	UPDATE
		ULS.phonesucsn.PhoneSuccession
	SET
		InvalidatedAt = GETDATE()
	WHERE
		PhoneSuccessionId = @PhoneSuccessionid