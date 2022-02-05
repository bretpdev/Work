CREATE PROCEDURE [faq].[PortfolioDelete]
	@PortfolioId int
AS
	delete from faq.Portfolios
	 where PortfolioId = @PortfolioId
RETURN 0
grant execute on [faq].[PortfolioDelete] to [db_executor]
