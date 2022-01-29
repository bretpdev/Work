CREATE PROCEDURE [dbo].[GetLetterTrackingData]
AS

	SELECT
		Request [TicketNumber],
		Title [Title],
		DocName [RequestName],
		Court [Court]
	FROM
		[BSYS].[dbo].[LTDB_DAT_Requests]

RETURN 0

GRANT EXECUTE ON GetLetterTrackingData TO db_executor