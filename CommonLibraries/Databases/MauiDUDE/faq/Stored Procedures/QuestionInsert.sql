CREATE PROCEDURE [faq].[QuestionInsert]
	@QuestionGroupId int,
	@Question nvarchar(4000),
	@Answer nvarchar(4000),
	@PortfolioIds PortfolioIds readonly
AS
	insert into faq.Questions (QuestionGroupId, Question, Answer)
	values (@QuestionGroupId, @Question, @Answer)

	declare @QuestionId int = SCOPE_IDENTITY()
	insert into faq.QuestionPortfolios (QuestionId, PortfolioId)
	select @QuestionId, PortfolioId
	  from @PortfolioIds
SELECT @QuestionId
grant execute on [faq].[QuestionInsert] to [db_executor]