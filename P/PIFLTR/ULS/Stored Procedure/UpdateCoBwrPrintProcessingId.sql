CREATE PROCEDURE [pifltr].[UpdateCoBwrPrintProcessingId]
	@ProcessQueueId INT,
	@CoBwrPrintProcessingId INT
AS
	UPDATE 
		ULS.pifltr.ProcessingQueue 
	SET 
		CoBwrPrintProcessingId = @CoBwrPrintProcessingId
	WHERE 
		ProcessQueueId = @ProcessQueueId --Loan specific due to TaskControlNumber
		AND CoBwrPrintProcessingId IS NULL

RETURN 0

GO
