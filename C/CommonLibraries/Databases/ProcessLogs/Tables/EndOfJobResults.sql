CREATE TABLE [dbo].[EndOfJobResults]
(
	[EndOfJobResultsId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [ProcessLogId] INT NOT NULL,
    [ResultHeader] VARCHAR(256) NOT NULL, 
    [ResultsValue] VARBINARY(256) NOT NULL, 
    CONSTRAINT [FK_EndOfJobResutls_ToTable] FOREIGN KEY (ProcessLogId) REFERENCES [ProcessLogs](ProcessLogId)
)
