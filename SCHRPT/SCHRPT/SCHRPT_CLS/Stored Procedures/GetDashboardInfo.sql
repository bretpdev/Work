CREATE PROCEDURE [schrpt].[GetDashboardInfo]
AS

	DECLARE @SchoolCount INT = (SELECT COUNT(*) FROM schrpt.Schools WHERE DeletedAt IS NULL)
	DECLARE @RecipientCount INT = (SELECT COUNT(*) FROM schrpt.Recipients WHERE DeletedAt IS NULL)

	SELECT
		@SchoolCount SchoolCount,
		@RecipientCount RecipientCount

RETURN 0
