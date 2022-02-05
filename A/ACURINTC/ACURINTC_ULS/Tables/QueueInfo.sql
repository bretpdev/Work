CREATE TABLE [acurintc].[QueueInfo]
(
	[QueueInfoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Queue] VARCHAR(20) NOT NULL, 
    [SubQueue] VARCHAR(20) NOT NULL, 
    [DemographicsReviewQueue] VARCHAR(20) NOT NULL, 
    [ForeignReviewQueue] VARCHAR(20) NOT NULL, 
    [ParserId] INT NOT NULL, 
    [ProcessorId] INT NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_QueueInfo_Parsers] FOREIGN KEY (ParserId) REFERENCES acurintc.Parsers(ParserId),
	CONSTRAINT [FK_QueueInfo_Processors] FOREIGN KEY (ProcessorId) REFERENCES acurintc.Processors(ProcessorId),
)
