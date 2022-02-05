CREATE TABLE [i1i2schltr].[RunDates]
(
	[RunDateId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RunDate] DATE NOT NULL,
	[AddedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(100) NULL,
	[DeletedAt] DATETIME NULL
)
