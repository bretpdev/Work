CREATE TABLE [dbo].[SystemLetterExclusions]
(
	[SystemLetterExclusionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LetterId] VARCHAR(10) NOT NULL, 
    [SystemLetterExclusionReasonId] INT NOT NULL, 
    CONSTRAINT [FK_SystemLetterExclusions_ToSystemLetterExclusionReasons] FOREIGN KEY (SystemLetterExclusionReasonId) REFERENCES SystemLetterExclusionReasons(SystemLetterExclusionReasonId)
)
