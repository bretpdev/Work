CREATE PROCEDURE [faq].[QuestionGroupDelete]
	@QuestionGroupId int
AS
	delete from faq.QuestionGroups
	 where QuestionGroupId = @QuestionGroupId
RETURN 0