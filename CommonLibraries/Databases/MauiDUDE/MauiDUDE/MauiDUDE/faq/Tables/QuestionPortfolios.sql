CREATE TABLE [faq].[QuestionPortfolios] (
    [QuestionPortfolioId] INT IDENTITY (1, 1) NOT NULL,
    [QuestionId]          INT NOT NULL,
    [PortfolioId]         INT NOT NULL,
    PRIMARY KEY CLUSTERED ([QuestionPortfolioId] ASC),
    CONSTRAINT [FK_QuestionPortfolios_Portfolios] FOREIGN KEY ([PortfolioId]) REFERENCES [faq].[Portfolios] ([PortfolioId]) ON DELETE CASCADE,
    CONSTRAINT [FK_QuestionPortfolios_Questions] FOREIGN KEY ([QuestionId]) REFERENCES [faq].[Questions] ([QuestionId]) ON DELETE CASCADE
);

