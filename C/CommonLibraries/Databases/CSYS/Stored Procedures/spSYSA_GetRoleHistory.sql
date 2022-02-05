-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/22/2012
-- Description:	Retrieves all the roles and their history
-- =============================================
CREATE PROCEDURE spSYSA_GetRoleHistory 
	-- Add the parameters for the stored procedure here
	@IsHistory bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @IsHistory = 0
		SELECT RoleID
			, RoleName
			, b.FirstName + ' ' + b.LastName AS [AddedBy]
			, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
		FROM SYSA_LST_Role a
		JOIN SYSA_DAT_Users b
		ON a.AddedBy = b.SqlUserId
		WHERE RemovedBy IS NULL
	ELSE
		SELECT RoleID
			, RoleName
			, b.FirstName + ' ' + b.LastName AS [AddedBy]
			, CONVERT(VARCHAR(30), a.StartDate, 120) AS [StartDate]
			, c.FirstName + ' ' + c.LastName AS [RemovedBy]
			, CONVERT(VARCHAR(30), a.EndDate, 120) AS [EndDate]
		FROM SYSA_LST_Role a
		LEFT JOIN SYSA_DAT_Users b
		ON a.AddedBy = b.SqlUserId
		LEFT JOIN SYSA_DAT_Users c
		ON a.RemovedBy = c.SqlUserId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetRoleHistory] TO [db_executor]
    AS [dbo];

