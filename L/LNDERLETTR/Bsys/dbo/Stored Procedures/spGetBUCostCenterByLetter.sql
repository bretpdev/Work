CREATE PROCEDURE [dbo].[spGetBUCostCenterByLetter]
	@LetterName varchar(10)

AS
	SELECT DISTINCT
		CP.[UHEAACostCenter], DD.Unit
	FROM
		BSYS.[dbo].LTDB_DAT_CentralPrintingDocData CP
		INNER JOIN 
			BSYS..LTDB_DAT_DocDetail DD
		ON
			CP.DocSeqNo = DD.DocSeqNo
	WHERE
		CP.ID = @LetterName
--RETURN 0