CREATE PROCEDURE [faq].[PortfolioInsert]
	@PortfolioName nvarchar(50)
AS
	insert into faq.Portfolios (PortfolioName)
	values (@PortfolioName)
RETURN 0
grant execute on [faq].[PortfolioInsert] to [db_executor]