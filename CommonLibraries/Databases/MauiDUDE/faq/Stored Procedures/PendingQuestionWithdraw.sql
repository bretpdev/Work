CREATE PROCEDURE [faq].[PendingQuestionWithdraw]
	@PendingQuestionId int
AS
	update faq.PendingQuestions
	   set ProcessedOn = getdate(), WithdrawnBy = SYSTEM_USER
	 where PendingQuestionId = @PendingQuestionId
RETURN 0
grant execute on [faq].[PendingQuestionWithdraw] to [db_executor]