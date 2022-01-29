CREATE PROCEDURE [dbo].[GetLetterTrackingData]
AS
	SELECT
		Request [TicketNumber],
		Title [Title],
		DocName [RequestName],
		Court [Court]
	FROM
		[BSYS].[dbo].[LTDB_DAT_Requests]