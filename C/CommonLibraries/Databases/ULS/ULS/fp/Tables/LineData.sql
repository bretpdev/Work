CREATE TABLE [fp].[LineData]
(
	[LineDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileProcessingId] INT NOT NULL, 
    [LineData] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_LineData_FileProcessing] FOREIGN KEY ([FileProcessingId]) REFERENCES fp.[FileProcessing]([FileProcessingId]) 
)