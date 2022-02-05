CREATE TABLE [projectrequest].[ScoreTypes]
(
	[ScoreTypeId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ScoreType] VARCHAR(50) NOT NULL,
	[Weight] INT NOT NULL,
	[OutOf] INT NOT NULL
)
