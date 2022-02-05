CREATE PROCEDURE [schrpt].[AddRecipient]
	@Name VARCHAR(50),
	@Email VARCHAR(256),
	@CompanyName VARCHAR(50) = NULL,
	@WindowsUserName VARCHAR(50)
AS

	INSERT INTO schrpt.Recipients (Name, Email, CompanyName, AddedBy)
	VALUES (@Name, @Email, @CompanyName, @WindowsUserName)

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0
