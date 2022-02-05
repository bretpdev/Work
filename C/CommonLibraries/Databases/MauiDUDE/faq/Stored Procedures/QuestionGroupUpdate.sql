CREATE PROCEDURE [faq].[QuestionGroupUpdate]
	@QuestionGroupId int,
	@GroupName nvarchar(50)
AS
	update faq.QuestionGroups
	   set GroupName = @GroupName
	 where QuestionGroupId = @QuestionGroupId
RETURN 0
grant execute on [faq].[QuestionGroupUpdate] to [db_executor]