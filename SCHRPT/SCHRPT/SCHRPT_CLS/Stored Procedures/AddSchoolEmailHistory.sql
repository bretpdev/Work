CREATE PROCEDURE [schrpt].[AddSchoolEmailHistory]
	@SchoolRecipientId INT,
	@EmailSentAt DATETIME,
	@FileSent VARCHAR(1000),
	@WindowsUserName VARCHAR(50)
AS

	INSERT INTO schrpt.SchoolEmailHistory(SchoolRecipientId, EmailSentAt, FileSent, AddedBy)
	VALUES (@SchoolRecipientId, @EmailSentAt, @FileSent, @WindowsUserName)

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0
