CREATE TABLE [calls].[CallRecords]
(
	[CallRecordId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReasonId] INT NOT NULL, 
	[Comments] nvarchar(30) NULL,
	[LetterID] varchar(10) NULL,
	[IsCornerstone] BIT NOT NULL,
	[IsOutbound] BIT NOT NULL,
    [RecordedOn] DATETIME NOT NULL DEFAULT getdate(), 
    [RecordedBy] NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_CallRecords_Reasons] FOREIGN KEY ([ReasonId]) REFERENCES [calls].Reasons([ReasonId])
)
