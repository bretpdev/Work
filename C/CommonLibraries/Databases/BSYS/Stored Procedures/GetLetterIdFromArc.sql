CREATE PROCEDURE [dbo].[GetLetterIdFromArc]
	@Arc varchar(5)
AS
	SELECT 
		DD.ID
	FROM
		LTDB_DAT_DocDetail DD
	LEFT JOIN LTDB_DAT_CentralPrintingDocData CP	
		ON CP.ID = DD.ID
	WHERE
		ARC = @Arc
		AND (DD.DocName like '%FED%' OR CP.UHEAACostCenter = 'MA4481')
RETURN 0
