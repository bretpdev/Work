CREATE PROCEDURE [faq].[PortfoliosSelectByQuestion]
	@QuestionId int
AS
	select p.PortfolioId, p.PortfolioName
	  from faq.Portfolios p
	  join faq.QuestionPortfolios qp on p.PortfolioId = qp.PortfolioId
	 where qp.QuestionId = @QuestionId
RETURN 0
grant execute on [faq].[PortfoliosSelectByQuestion] to [db_executor]