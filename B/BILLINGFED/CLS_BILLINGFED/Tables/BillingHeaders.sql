CREATE TABLE [billing].[BillingHeaders] (
    [BillingHeaderId] INT            IDENTITY (1, 1) NOT NULL,
    [BillingHeader]   VARCHAR (1000) NOT NULL,
    [AddedAt]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [InactivatedAt]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([BillingHeaderId] ASC)
);

