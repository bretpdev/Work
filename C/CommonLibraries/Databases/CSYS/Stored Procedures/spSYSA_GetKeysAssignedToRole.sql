-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/21/2012
-- Description:	Returns list of access keys for given role
-- =============================================
CREATE PROCEDURE [dbo].[spSYSA_GetKeysAssignedToRole] 
	-- Add the parameters for the stored procedure here
	  @RoleID int
	, @IsHistory bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @IsHistory = 0
		SELECT d.RoleName
			, b.ID
			, b.UserKey AS [Name]
			, b.[Application]
			, b.[Description]
			, c.FirstName + ' ' + c.LastName AS [AddedBy]
			, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
		FROM SYSA_DAT_RoleKeyAssignment a
		LEFT JOIN SYSA_LST_UserKeys b
		ON a.UserKeyID = b.ID
		LEFT JOIN SYSA_DAT_Users c
		ON a.AddedBy = c.SqlUserId
		JOIN SYSA_LST_Role d
		ON a.RoleID = d.RoleID
		WHERE a.RoleID = @RoleID
		AND a.RemovedBy IS NULL
	ELSE
		SELECT e.RoleName
			, b.ID
			, b.UserKey AS [Name]
			, b.[Application]
			, b.[Description]
			, c.FirstName + ' ' + c.LastName AS [AddedBy]
			, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
			, d.FirstName + ' ' + d.LastName AS [RemovedBy]
			, CONVERT(VARCHAR(30), a.EndDate, 120) AS [EndDate]
		FROM SYSA_DAT_RoleKeyAssignment a
		LEFT JOIN SYSA_LST_UserKeys b
		ON a.UserKeyID = b.ID
		LEFT JOIN SYSA_DAT_Users c
		ON a.AddedBy = c.SqlUserId
		LEFT JOIN SYSA_DAT_Users d
		ON a.RemovedBy = d.SqlUserId
		JOIN SYSA_LST_Role e
		ON a.RoleID = e.RoleID
		WHERE a.RoleID = @RoleID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetKeysAssignedToRole] TO [db_executor]
    AS [dbo];

