CREATE TYPE [dbo].[UserData] AS TABLE
(
	[RunDateTime] DateTime,
	[Queue] varchar(8),
	[UserId] varchar(8),
	[Assigned] int,
	[Complete] int,
	[Canceled] int
)
