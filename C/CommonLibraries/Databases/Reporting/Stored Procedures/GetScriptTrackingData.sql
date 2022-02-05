CREATE PROCEDURE [dbo].[GetScriptTrackingData]
AS

	SELECT
		SR.Request [TicketNumber],
		S.Script [Title],
		SR.Title [RequestName],
		SR.Court [Court]
	FROM
		[BSYS].[dbo].SCKR_DAT_ScriptRequests SR
		LEFT JOIN [BSYS].[dbo].SCKR_DAT_Scripts S ON SR.Script = S.Script

RETURN 0

GRANT EXECUTE ON GetScriptTrackingData TO db_executor