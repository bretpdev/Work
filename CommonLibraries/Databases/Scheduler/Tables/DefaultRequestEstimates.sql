CREATE TABLE [dbo].[DefaultRequestEstimates] (
    [DefaultRequestEstimateId] INT            IDENTITY (1, 1) NOT NULL,
    [RequestTypeId]            INT            NULL,
    [DevEstimate]              DECIMAL (6, 2) NULL,
    [TestEstimate]             DECIMAL (6, 2) NULL,
    PRIMARY KEY CLUSTERED ([DefaultRequestEstimateId] ASC),
    FOREIGN KEY ([RequestTypeId]) REFERENCES [dbo].[RequestTypes] ([RequestTypeId])
);

