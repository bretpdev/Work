CREATE PROCEDURE [faq].[PendingQuestionsSelectByUser]
	@UserName nvarchar(50)
AS
	SELECT PendingQuestionId, Question, Notes, SubmittedOn, SubmittedBy, ProcessedOn, ApprovedBy, RejectedBy, WithdrawnBy
	  from faq.PendingQuestions
	 where SubmittedBy = @UserName
	   and WithdrawnBy is null
	 order by SubmittedOn desc
RETURN 0
grant execute on [faq].[PendingQuestionsSelectByUser] to [db_executor]