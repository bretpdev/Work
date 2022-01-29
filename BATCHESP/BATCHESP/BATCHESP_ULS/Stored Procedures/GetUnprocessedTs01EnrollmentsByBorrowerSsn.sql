CREATE PROCEDURE [batchesp].[GetUnprocessedTs01EnrollmentsByBorrowerSsn]
	@BorrowerSSN CHAR(9)
AS
	SELECT
		TS01EnrollmentId,
		BorrowerSSN,		
		LoanSequence,		
		StudentSSN,			
		SeparationDate,		
		SchoolCode,			
		SeparationReason,	
		SeparationSource,	
		DateNotified,		
		DateCertified		
	FROM
		[batchesp].[TS01Enrollments]
	WHERE
		ProcessedAt IS NULL
		AND BorrowerSSN = @BorrowerSSN

RETURN 0
