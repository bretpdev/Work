CREATE PROCEDURE [phonesucsn].[SetError]
	@PhoneSuccessionId INT
AS
	UPDATE
		phonesucsn.PhoneSuccession
	SET
		HadError = 1
	WHERE
		PhoneSuccessionId = @PhoneSuccessionId
RETURN 0