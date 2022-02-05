
CREATE PROCEDURE [dbo].[spLTDB_GetLetterInfo]
	@LetterId			VARCHAR(10) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT		CAST ((CASE 
					WHEN Instructions IS NULL or CAST(Instructions AS VARCHAR(50)) = '' THEN 0
					ELSE 1
				END) AS BIT) AS SpecialHandling,
				CASE 
					WHEN Duplex = 1 THEN CEILING(Pages / 2)
					ELSE Pages
				END AS Pages,
				UHEAACostCenter AS CostCenter,
				Duplex, 
				ID AS LetterId,
				Instructions
	FROM		dbo.LTDB_DAT_CentralPrintingDocData
	WHERE		(@LetterId IS NULL OR ID = @LetterId)
END