CREATE PROCEDURE [faq].[PendingQuestionInsert]
	@Question nvarchar(4000),
	@Notes nvarchar(4000)
AS
	insert into faq.PendingQuestions (Question, Notes)
	values (@Question, @Notes)
RETURN 0