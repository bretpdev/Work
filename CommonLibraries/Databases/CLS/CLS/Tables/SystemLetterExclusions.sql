CREATE TABLE [dbo].[SystemLetterExclusions]
(
	[SystemLetterExclusionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LetterId] VARCHAR(10) NOT NULL, 
    [SystemLetterExclusionReasonId] INT NULL , 
    CONSTRAINT [FK_SystemLetterExclusions_ToTable] FOREIGN KEY (SystemLetterExclusionReasonId) REFERENCES [SystemLetterExclusionReasons](SystemLetterExclusionReasonId)
)
