CREATE PROCEDURE [faq].[QuestionUpdate]
	@QuestionId int,
	@QuestionGroupId int,
	@Question nvarchar(4000),
	@Answer nvarchar(4000),
	@PortfolioIds PortfolioIds readonly
AS
	update faq.Questions
	   set QuestionGroupId = @QuestionGroupId, Question = @Question, Answer = @Answer
	 where QuestionId = @QuestionId

	delete from faq.QuestionPortfolios where QuestionId = @QuestionId

	insert into faq.QuestionPortfolios (QuestionId, PortfolioId)
	select @QuestionId, PortfolioId
	  from @PortfolioIds
RETURN 0