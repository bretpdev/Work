CREATE TABLE [rtnemlinvf].[InvalidEmail]
(
	[InvalidEmailId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] CHAR(9) NOT NULL, 
    [EmailType] CHAR NOT NULL, 
    [ReceivedBy] VARCHAR(254) NOT NULL, 
    [ReceivedDate] DATETIME NOT NULL, 
    [Subject] VARCHAR(1000) NOT NULL, 
    [InvalidatedAt] DATETIME NULL,
    [ArcAddProcessingId] BIGINT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [FK_InvalidEmail_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing]([ArcAddProcessingId])
)
