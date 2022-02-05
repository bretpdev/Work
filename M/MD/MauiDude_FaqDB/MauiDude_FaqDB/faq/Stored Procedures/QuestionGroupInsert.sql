CREATE PROCEDURE [faq].[QuestionGroupInsert]
	@GroupName nvarchar(50)
AS
	insert into faq.QuestionGroups (GroupName)
	values (@GroupName)
RETURN 0