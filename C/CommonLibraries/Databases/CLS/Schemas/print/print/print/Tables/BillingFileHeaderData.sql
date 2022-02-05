CREATE TABLE [print].[BillingFileHeaderData] (
    [BillingFileHeaderDataId] INT IDENTITY (1, 1) NOT NULL,
    [FileHeaderId]            INT NOT NULL,
    [BillDueDateIndex]        INT NOT NULL,
    [TotalDueIndex]           INT NOT NULL,
    [BillSeqIndex]            INT NOT NULL,
    [BillCreateDateIndex]     INT NULL,
    PRIMARY KEY CLUSTERED ([BillingFileHeaderDataId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BillingFileHeaderData_ToFileHeader] FOREIGN KEY ([FileHeaderId]) REFERENCES [print].[FileHeaders] ([FileHeaderId])
);

