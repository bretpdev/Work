CREATE TABLE [fp].[LineData] (
    [LineDataId]       INT           IDENTITY (1, 1) NOT NULL,
    [FileProcessingId] INT           NOT NULL,
    [LineData]         VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([LineDataId] ASC),
    CONSTRAINT [FK_LineData_FileProcessing] FOREIGN KEY ([FileProcessingId]) REFERENCES [fp].[FileProcessing] ([FileProcessingId])
);

