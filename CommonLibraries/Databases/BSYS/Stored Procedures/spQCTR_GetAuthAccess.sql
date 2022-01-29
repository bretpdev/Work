CREATE PROCEDURE [dbo].[spQCTR_GetAuthAccess] 

@WindowsUserId		nvarchar(50)

AS
BEGIN

SET NOCOUNT ON;

SELECT CASE WHEN BusinessUnit IS NULL OR BusinessUnit = '' THEN AccessNotificationKey ELSE BusinessUnit + ' - ' + AccessNotificationKey END AS Access
FROM dbo.SYSA_DAT_AccessAndNotificationUserAccess
WHERE WindowsUserID = @WindowsUserId AND [System] = 'Quality Control' AND EndDate IS NULL


END