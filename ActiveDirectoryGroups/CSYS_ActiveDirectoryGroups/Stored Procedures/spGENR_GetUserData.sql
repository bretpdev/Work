-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/28/2012
-- Description:	Returns a list of all the users in the SYSA_DAT_Users table
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetUserData] 
	@Status varchar(50) = ''
AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Status <> ''
		SELECT
			U.SqlUserID
			,U.WindowsUserName
			,U.FirstName
			,U.MiddleInitial
			,U.LastName
			,U.EMail
			,U.Extension 
			,U.Extension2
			,BU.[Name] [BusinessUnit]
			,R.RoleName [Role]
			,U.[Status]
			,U.Title
			,U.AesUserId
		FROM
			SYSA_DAT_Users U
			LEFT JOIN GENR_LST_BusinessUnits BU
				ON U.BusinessUnit = BU.ID
			LEFT JOIN SYSA_LST_Role R
				ON U.[Role] = R.RoleID
		WHERE
			[Status] = @Status
	ELSE
				SELECT
			U.SqlUserID
			,U.WindowsUserName
			,U.FirstName
			,U.MiddleInitial
			,U.LastName
			,U.EMail
			,U.Extension 
			,U.Extension2
			,BU.[Name] [BusinessUnit]
			,R.RoleName [Role]
			,U.[Status]
			,U.Title
			,U.AesUserId
		FROM
			SYSA_DAT_Users U
			LEFT JOIN GENR_LST_BusinessUnits BU
				ON U.BusinessUnit = BU.ID
			LEFT JOIN SYSA_LST_Role R
				ON U.[Role] = R.RoleID
END