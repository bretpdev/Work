CREATE PROCEDURE [batchesp].[GetUnprocessedDefermentForbearancesByBorrowerSsn]
	@BorrowerSsn CHAR(9)
AS
	SELECT
		TsayDefermentForbearanceId,
		BorrowerSSN,
		LoanSequence,				
		[Type],						
		BeginDate,					
		EndDate,				
		CertificationDate,	
		DeferSchool,
		RequestedBeginDate,
		RequestedEndDate
	FROM
		[batchesp].[TsayDefermentForbearances]
	WHERE
		ProcessedAt IS NULL
		AND BorrowerSsn = @BorrowerSsn

RETURN 0
