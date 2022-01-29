CREATE TABLE [projectrequest].[Scores]
(
	[ScoreId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProjectId] INT NOT NULL,
	[ScoreTypeId] INT NOT NULL,
	[Score] INT NOT NULL,
	[Scorer] VARCHAR(50) NOT NULL,
	[ScoreDate] DATETIME NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1,
	[PreviousScoreId] INT NULL,
	CONSTRAINT [FK_Scores_Projects] FOREIGN KEY ([ProjectId]) REFERENCES projectrequest.Projects([ProjectId]),
	CONSTRAINT [FK_Scores_ScoreTypes] FOREIGN KEY ([ScoreTypeId]) REFERENCES projectrequest.ScoreTypes([ScoreTypeId]),
	CONSTRAINT [FK_Scores_Scores] FOREIGN KEY ([PreviousScoreId]) REFERENCES projectrequest.Scores([ScoreId])
)
