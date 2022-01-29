CREATE PROCEDURE [faq].[PortfoliosSelectAll]
AS
	select PortfolioId, PortfolioName
	  from faq.Portfolios
RETURN 0
grant execute on [faq].[PortfoliosSelectAll] to [db_executor]
