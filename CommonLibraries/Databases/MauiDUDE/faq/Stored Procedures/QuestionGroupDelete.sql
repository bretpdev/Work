CREATE PROCEDURE [faq].[QuestionGroupDelete]
	@QuestionGroupId int
AS
	delete from faq.QuestionGroups
	 where QuestionGroupId = @QuestionGroupId
RETURN 0
grant execute on [faq].[QuestionGroupDelete] to [db_executor]