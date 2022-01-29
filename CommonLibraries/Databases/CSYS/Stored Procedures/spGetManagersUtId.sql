-- =============================================
-- Author:		Jarom Ryan
-- Create date: 08/07/2013
-- Description:	Gets the UT id for the manager of the given BU
-- =============================================
CREATE PROCEDURE [dbo].[spGetManagersUtId]

@BusinessUnitId INTEGER

AS
BEGIN

	SET NOCOUNT ON;


	SELECT
		AesUserId
	FROM 
		SYSA_DAT_Users
	WHERE 
		BusinessUnit = @BusinessUnitId
		AND Title = 'Manager'
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetManagersUtId] TO [db_executor]
    AS [dbo];

