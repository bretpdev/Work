CREATE TABLE [billing].[LineData] (
    [LineDataId]        INT           IDENTITY (1, 1) NOT NULL,
    [PrintProcessingId] INT           NOT NULL,
    [LineData]          VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([LineDataId] ASC),
    FOREIGN KEY ([PrintProcessingId]) REFERENCES [billing].[PrintProcessing] ([PrintProcessingId])
);

