CREATE PROCEDURE dbo.spNDHP_GetAuthAccess 

@WindowsUserId		nvarchar(50)

AS

SELECT CASE WHEN BusinessUnit IS NULL OR BusinessUnit = '' THEN Access ELSE BusinessUnit + ' - ' + Access END AS Access
FROM dbo.NDHP_REF_AuthAccess
WHERE WindowsUserID = @WindowsUserId