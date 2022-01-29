CREATE PROCEDURE [faq].[PortfolioUpdate]
	@PortfolioId int,
	@PortfolioName nvarchar(50)
AS
	update faq.Portfolios
	   set PortfolioName = @PortfolioName
	 where PortfolioId = @PortfolioId
RETURN 0
grant execute on [faq].[PortfolioUpdate] to [db_executor]