CREATE PROCEDURE [faq].[QuestionDelete]
	@QuestionId int
AS
	delete from faq.Questions
	 where QuestionId = @QuestionId
RETURN 0
grant execute on [faq].[QuestionDelete] to [db_executor]
