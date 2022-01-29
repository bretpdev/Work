CREATE PROCEDURE [faq].[InsertLog]
	@QuestionGroupId INT,
	@QuestionId VARCHAR(4000)
AS
	INSERT INTO faq.Logs(QuestionGroupsId, QuestionsId)
	VALUES(@QuestionGroupId, @QuestionId)
RETURN 0