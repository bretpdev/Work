CREATE PROCEDURE [pridrcrp].[CheckExistingArc]
(
	@AccountNumber VARCHAR(10),
	@Arc VARCHAR(5)
)
AS

SELECT
	 AAP.[ArcAddProcessingId]
FROM 
	[CLS].[dbo].[ArcAddProcessing] AAP
WHERE
	AAP.AccountNumber = @AccountNumber
	AND AAP.ARC = @Arc
	AND 
	(
		AAP.ProcessedAt IS NULL
		OR
		(
			CAST(AAP.CreatedAt AS DATE) >= CAST(DATEADD(DAY, -1, GETDATE()) AS DATE)
		)
	)