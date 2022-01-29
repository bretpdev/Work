CREATE PROCEDURE [dbo].[GetUserId]
	@WindowsUserName VARCHAR(200)
AS
	SELECT 
		UserID 
	from 
		SYSA_LST_UserIDInfo 
	where 
		WindowsUserName = @WindowsUserName
		AND [Date Access Removed] IS NULL 
		AND UserID like 'UT%'
RETURN 0
