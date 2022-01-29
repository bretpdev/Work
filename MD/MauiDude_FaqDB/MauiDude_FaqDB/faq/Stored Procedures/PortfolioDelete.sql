CREATE PROCEDURE [faq].[PortfolioDelete]
	@PortfolioId int
AS
	delete from faq.Portfolios
	 where PortfolioId = @PortfolioId
RETURN 0