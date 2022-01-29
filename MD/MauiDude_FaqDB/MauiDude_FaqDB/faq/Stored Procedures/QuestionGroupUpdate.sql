CREATE PROCEDURE [faq].[QuestionGroupUpdate]
	@QuestionGroupId int,
	@GroupName nvarchar(50)
AS
	update faq.QuestionGroups
	   set GroupName = @GroupName
	 where QuestionGroupId = @QuestionGroupId
RETURN 0