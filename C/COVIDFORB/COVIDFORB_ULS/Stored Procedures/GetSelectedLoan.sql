CREATE PROCEDURE covidforb.[GetSelectedLoans]
(
	@ForbProcessingId BIGINT
)
AS

SELECT
	FLSS.LoanSequence
FROM 
	[covidforb].ForbLoanSequenceSelection FLSS
WHERE
	FLSS.ForbearanceProcessingId = @ForbProcessingId