CREATE TABLE [billing].[BillingData] (
    [BillingDataId]  INT IDENTITY (1, 1) NOT NULL,
    [LineDataId]     INT NOT NULL,
    [DaysDelinquent] INT NULL,
    PRIMARY KEY CLUSTERED ([BillingDataId] ASC),
    FOREIGN KEY ([LineDataId]) REFERENCES [billing].[LineData] ([LineDataId])
);

