CREATE TABLE [fp].[LineData] (
    [LineDataId]       INT           NOT NULL IDENTITY,
    [FileProcessingId] INT           NOT NULL,
    [LineData]         VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([LineDataId] ASC),
    CONSTRAINT [FK_LineData_FileProcessing] FOREIGN KEY ([FileProcessingId]) REFERENCES [fp].[FileProcessing] ([FileProcessingId])
);

