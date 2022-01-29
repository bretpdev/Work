CREATE PROCEDURE [faq].[PendingQuestionsSelectPending]
AS
	select PendingQuestionId, Question, Notes, SubmittedOn, SubmittedBy
	  from faq.PendingQuestions
	 where ProcessedOn is null
RETURN 0
grant execute on [faq].[PendingQuestionsSelectPending] to [db_executor]
