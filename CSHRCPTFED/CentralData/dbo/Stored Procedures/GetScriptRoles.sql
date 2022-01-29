-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetScriptRoles]
	@ScriptId VARCHAR(48)
AS
BEGIN
		SELECT
			RoleId
		FROM 
			 [CentralData].[dbo].[ActiveDirRoles]
		WHERE
			ScriptId = @ScriptId
		AND
			DeletedAt IS NULL
		AND 
			DeletedBy IS NULL

END