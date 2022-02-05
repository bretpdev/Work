CREATE TABLE [projectrequest].[Permissions]
(
	[PermissionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Read] BIT NOT NULL,
	[Create] BIT NOT NULL,
	[Score] BIT NOT NULL,
	[ScoreFinance] BIT NOT NULL,
	[ScoreRequestor] BIT NOT NULL,
	[ScoreUrgency] BIT NOT NULL,
	[ScoreResources] BIT NOT NULL,
	[ScoreRisk] BIT NOT NULL,
	[Archive] BIT NOT NULL,
	[Admin] BIT NOT NULL
)
