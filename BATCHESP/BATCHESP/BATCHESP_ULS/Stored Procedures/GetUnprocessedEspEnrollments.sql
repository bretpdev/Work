CREATE PROCEDURE [batchesp].[GetUnprocessedEspEnrollments]
	
AS
	SELECT
		EspEnrollmentId,
		BorrowerSSN,		
		AccountNumber,			
		[Queue],				
		SubQueue,				
		TaskControlNumber,		
		Arc,	
		ArcRequestDate,			
		Message1,				
		SupplementalMessage,	
		StudentSSN,			
		StudentSSN2,			
		SchoolCode,			
		ESP_Status,				
		ESP_SeparationDate,		
		ESP_CertificationDate,	
		EnrollmentBeginDate,	
		SourceCode,		
		RequiresReview		
	FROM
		[batchesp].[ESPEnrollments]
	WHERE
		ProcessedAt IS NULL

RETURN 0
