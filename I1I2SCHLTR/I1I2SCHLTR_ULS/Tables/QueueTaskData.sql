CREATE TABLE [i1i2schltr].[QueueTaskData]
(
	[QueueTaskDataId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RunDateId] INT NOT NULL,
	[SSN] VARCHAR(9) NOT NULL,
	[Queue] VARCHAR(2) NULL,
	[AddedAt] DATETIME NULL,
	[ProcessedAt] DATETIME NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL,
	[ProcessingAttempts] INT NOT NULL DEFAULT(0),
	CONSTRAINT FK_QueueTaskData_RunDates FOREIGN KEY(RunDateId) REFERENCES i1i2schltr.RunDates(RunDateId)
)
