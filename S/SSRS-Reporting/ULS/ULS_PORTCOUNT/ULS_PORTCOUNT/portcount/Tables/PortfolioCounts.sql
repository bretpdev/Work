CREATE TABLE [portcount].[PortfolioCounts] (
    [PortfolioCountsId]  INT          IDENTITY (1, 1) NOT NULL,
    [UheaaLoanCount]     BIGINT          NOT NULL,
    [UheaaBorrowerCount] BIGINT          NOT NULL,
    [CreatedAt]          DATE     DEFAULT (GETDATE()) NOT NULL,
    [CreatedBy]          VARCHAR (50) DEFAULT (SUSER_SNAME()) NOT NULL,
    CONSTRAINT [IX_PortfolioCounts_Unique] UNIQUE NONCLUSTERED ([CreatedAt] DESC) WITH (FILLFACTOR = 95)
);


