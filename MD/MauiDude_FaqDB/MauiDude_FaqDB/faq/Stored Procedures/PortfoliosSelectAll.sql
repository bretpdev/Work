CREATE PROCEDURE [faq].[PortfoliosSelectAll]
AS
	select PortfolioId, PortfolioName
	  from faq.Portfolios
RETURN 0