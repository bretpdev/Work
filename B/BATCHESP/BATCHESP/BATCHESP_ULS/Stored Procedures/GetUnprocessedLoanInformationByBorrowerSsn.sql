CREATE PROCEDURE [batchesp].[GetUnprocessedLoanInformationByBorrowerSsn]
	@BorrowerSSN CHAR(9)
AS
	SELECT
		TS26LoanInformationId,	
		BorrowerSSN,			
		LoanSequence,			
		RTRIM(LoanProgramType) [LoanProgramType],
		CurrentPrincipal,		
		DisbursementDate,		
		GraceEndDate,		
		RepaymentStartDate,		
		EffectAddDate,			
		RehabRepurch,			
		TermBeg,				
		TermEnd,	
		EffectAddDate			
	FROM
		[batchesp].[TS26LoanInformation]
	WHERE
		ProcessedAt IS NULL
		AND BorrowerSSN = @BorrowerSSN

RETURN 0
