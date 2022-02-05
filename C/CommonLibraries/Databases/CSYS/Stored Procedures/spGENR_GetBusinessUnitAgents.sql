-- =============================================
-- Author:		Daren Beattie
-- Create date: September 19, 2011
-- Description:	Retrieves business unit agents and their roles.
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetBusinessUnitAgents]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT a.BusinessUnit AS [BusinessUnitID]
		, a.SqlUserId
		, b.RoleName AS [Role]
	FROM SYSA_DAT_Users a
		JOIN SYSA_LST_Role b
		ON a.Role = b.RoleID
	WHERE a.Status = 'Active'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetBusinessUnitAgents] TO [db_executor]
    AS [dbo];

