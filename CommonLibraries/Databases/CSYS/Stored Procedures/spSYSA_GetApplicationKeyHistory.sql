
CREATE PROCEDURE [dbo].[spSYSA_GetApplicationKeyHistory]
	@IsHistory BIT
AS
BEGIN
	SET NOCOUNT ON;

	IF @IsHistory = 0
		SELECT a.ID
		, a.[Application]
		, a.UserKey AS Name
		, a.[Type]
		, a.[Description]
		, b.FirstName + ' ' + b.LastName AS [AddedBy]
		, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
		FROM SYSA_LST_UserKeys a
		LEFT JOIN SYSA_DAT_Users b
		ON a.AddedBy = b.SqlUserId
		WHERE a.EndDate IS NULL
		ORDER BY Name
	ELSE
		SELECT a.ID
		, a.[Application]
		, a.UserKey AS Name
		, a.[Type]
		, a.[Description]
		, b.FirstName + ' ' + b.LastName AS [AddedBy]
		, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
		, c.FirstName + ' ' + c.LastName AS [RemovedBy]
		, CONVERT(VARCHAR(20), a.EndDate, 120) AS [EndDate]
		FROM SYSA_LST_UserKeys a
		LEFT JOIN SYSA_DAT_Users b
		ON a.AddedBy = b.SqlUserId
		LEFT JOIN SYSA_DAT_Users c
		ON a.RemovedBy = c.SqlUserId
		ORDER BY Name, a.StartDate
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetApplicationKeyHistory] TO [db_executor]
    AS [dbo];

