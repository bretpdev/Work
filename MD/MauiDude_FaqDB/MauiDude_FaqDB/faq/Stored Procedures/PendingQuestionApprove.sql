CREATE PROCEDURE [faq].[PendingQuestionApprove]
	@PendingQuestionId int
AS
	update faq.PendingQuestions
	   set ProcessedOn = getdate(), ApprovedBy = SYSTEM_USER
	 where PendingQuestionId = @PendingQuestionId
RETURN 0