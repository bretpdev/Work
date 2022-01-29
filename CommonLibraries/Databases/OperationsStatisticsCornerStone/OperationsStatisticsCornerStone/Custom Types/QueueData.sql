CREATE TYPE [dbo].[QueueData] AS TABLE
(
	[RunDateTime] DateTime,
	[Queue] varchar(8),
	[Total] bigint,
	[Complete] bigint,
	[Critical] bigint,
	[Canceled] bigint,
	[OutStanding] bigint,
	[Problem] bigint,
	[Dept] varchar(3),
	[Late] bigint
)
