CREATE PROCEDURE [dbo].[GetAllScriptIds]

AS
	SELECT 
		ID
	FROM
		SCKR_DAT_Scripts
	WHERE
		[Status] NOT IN ( 'Withdrawn', 'Retired')
		AND ID IS NOT NULL
	ORDER BY 
		ID
RETURN 0
