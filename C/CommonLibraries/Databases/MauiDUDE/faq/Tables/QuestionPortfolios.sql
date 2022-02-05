CREATE TABLE [faq].[QuestionPortfolios]
(
	[QuestionPortfolioId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [QuestionId] INT NOT NULL, 
    [PortfolioId] INT NOT NULL, 
    CONSTRAINT [FK_QuestionPortfolios_Questions] FOREIGN KEY (QuestionId) REFERENCES [faq].[Questions]([QuestionId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_QuestionPortfolios_Portfolios] FOREIGN KEY ([PortfolioId]) REFERENCES [faq].[Portfolios]([PortfolioId]) ON DELETE CASCADE
)
