
CREATE PROCEDURE [faq].[RecordQuestion]
	@Question nvarchar(max)
AS
	
	insert into faq.RecordedQuestions (Question) values (@Question)

RETURN 0

