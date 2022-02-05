
/********************************************************
*Routine Name	: [dbo].[spSYSA_GetRoleAssignedKeys]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/25/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_GetRoleAssignedKeys]
	-- Add the parameters for the stored procedure here
	  @RoleID int,
	  @Application VARCHAR(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Application = '' OR @Application IS NULL
		SELECT a.ID, a.UserKey AS [Name]
		, a.[Application]
		, a.[Type]
		, a.[Description]
		, c.FirstName + ' ' + c.LastName AS [AddedBy]
		, CONVERT(VARCHAR(20), b.StartDate, 120) AS [StartDate]
		FROM SYSA_LST_UserKeys a
		JOIN SYSA_DAT_RoleKeyAssignment b
		ON a.ID = b.UserKeyID
		JOIN SYSA_DAT_Users c
		ON b.AddedBy = c.SqlUserId
		WHERE b.RoleID = @RoleID
		AND b.EndDate IS NULL
	ELSE
		SELECT a.ID
		, a.UserKey AS [Name]
		, a.[Application]
		, a.[Type]
		, a.[Description]
		, c.FirstName + ' ' + c.LastName AS [AddedBy]
		, CONVERT(VARCHAR(20), b.StartDate, 120) AS [StartDate]
		FROM SYSA_LST_UserKeys a
		JOIN SYSA_DAT_RoleKeyAssignment b
		ON a.ID = b.UserKeyID
		JOIN SYSA_DAT_Users c
		ON b.AddedBy = c.SqlUserId
		WHERE b.RoleID = @RoleID
		AND a.[Application] = @Application
		AND b.EndDate IS NULL

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetRoleAssignedKeys] TO [db_executor]
    AS [dbo];

