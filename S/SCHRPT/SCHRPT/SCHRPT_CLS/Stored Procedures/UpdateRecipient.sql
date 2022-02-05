CREATE PROCEDURE [schrpt].[UpdateRecipient]
	@RecipientId INT,
	@Name VARCHAR(50),
	@Email VARCHAR(256),
	@CompanyName VARCHAR(50)
AS

	UPDATE r
	SET
		r.Name = @Name, r.Email = @Email, r.CompanyName = @CompanyName
	FROM
		schrpt.Recipients r
	WHERE
		r.RecipientId = @RecipientId

RETURN 0
