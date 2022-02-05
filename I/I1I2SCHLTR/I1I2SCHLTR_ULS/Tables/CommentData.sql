CREATE TABLE [i1i2schltr].[CommentData]
(
	[CommentDataId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RunDateId] INT NOT NULL,
	[SSN] VARCHAR(9) NOT NULL,
	[AddedAt] DATETIME NULL,
	[CommentProcessedAt] DATETIME NULL,
	[TaskProcessedAt] DATETIME NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL,
	[ProcessingAttempts] INT NOT NULL DEFAULT(0),
	CONSTRAINT FK_CommentData_RunDates FOREIGN KEY(RunDateId) REFERENCES i1i2schltr.RunDates(RunDateId)
)
