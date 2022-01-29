CREATE PROCEDURE [forb].[GetSelectedLoans]
(
	@ForbProcessingId BIGINT
)
AS

SELECT
	FLSS.LoanSequence
FROM 
	[forb].ForbLoanSequenceSelection FLSS
WHERE
	FLSS.ForbearanceProcessingId = @ForbProcessingId