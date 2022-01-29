CREATE PROCEDURE [schrpt].[AddSchoolRecipient]
	@SchoolId INT,
	@RecipientId INT,
	@ReportTypeId INT,
	@WindowsUserName VARCHAR(50)
AS
	INSERT INTO schrpt.SchoolRecipients (SchoolId, RecipientId, ReportTypeId, AddedBy)
	VALUES (@SchoolId, @RecipientId, @ReportTypeId, @WindowsUserName)

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0
