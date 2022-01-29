﻿CREATE TABLE [projectrequest].[Projects]
(
	[ProjectId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BusinessUnitId] INT NOT NULL,
	[FinanceScoreId] INT NULL,
	[RequestorScoreId] INT NULL,
	[UrgencyScoreId] INT NULL,
	[ResourcesScoreId] INT NULL,
	[RiskScoreId] INT NULL, 
	[ProjectName] VARCHAR(50) NOT NULL,
	[SubmittedBy] VARCHAR(50) NOT NULL,
	[SubmittedAt] DATETIME NOT NULL,
	[ProjectSummary] VARCHAR(MAX) NULL,
	[BusinessNeed] VARCHAR(MAX) NULL,
	[Benefits] VARCHAR(MAX) NULL,
	[ImplementationApproach] VARCHAR(MAX) NULL,
	[ProjectStatus] VARCHAR(100) NULL,
	[ProjectNumber] VARCHAR(100) NULL,
	[Archived] BIT NULL DEFAULT 0,
	[UpdatedAt] DATETIME NULL,
	[UpdatedBy] VARCHAR(50) NULL,
	[ArchivedAt] DATETIME NULL,
	[ArchivedBy] VARCHAR(50) NULL,
	CONSTRAINT [FK_Projects_BusinessUnits] FOREIGN KEY ([BusinessUnitId]) REFERENCES projectrequest.BusinessUnits([BusinessUnitId]),
	CONSTRAINT [FK_Projects_FinanceScore] FOREIGN KEY ([FinanceScoreId]) REFERENCES projectrequest.Scores([ScoreId]),
	CONSTRAINT [FK_Projects_RequestorScore] FOREIGN KEY ([RequestorScoreId]) REFERENCES projectrequest.Scores([ScoreId]),
	CONSTRAINT [FK_Projects_UrgencyScore] FOREIGN KEY ([UrgencyScoreId]) REFERENCES projectrequest.Scores([ScoreId]),
	CONSTRAINT [FK_Projects_ResourcesScore] FOREIGN KEY ([ResourcesScoreId]) REFERENCES projectrequest.Scores([ScoreId]),
	CONSTRAINT [FK_Projects_RiskScore] FOREIGN KEY ([RiskScoreId]) REFERENCES projectrequest.Scores([ScoreId])
)
