CREATE PROCEDURE [dbo].[RegeneratePreviousLetterAsAltFormat]
	@DocumentDetailsId int,
	@CorrespondenceFormatId int
AS
	INSERT INTO DocumentDetails([LetterId], [Path], [Ssn], [DocDate], [ADDR_ACCT_NUM], [RequestUser], [CorrMethod], [LoadTime], [AddresseeEmail], [CreateDate], [DueDate], [TotalDue], [BillSeq], [CorrespondenceFormatId])
	SELECT
		[LetterId],
		[Path], 
		[Ssn], 
		[DocDate], 
		[ADDR_ACCT_NUM], 
		[RequestUser],
		 [CorrMethod], 
		 [LoadTime], 
		 [AddresseeEmail], 
		 [CreateDate], 
		 [DueDate], 
		 [TotalDue], 
		 [BillSeq],
		 @CorrespondenceFormatId
	FROM	
		DocumentDetails
	WHERE
		DocumentDetailsId = @DocumentDetailsId
RETURN 0
