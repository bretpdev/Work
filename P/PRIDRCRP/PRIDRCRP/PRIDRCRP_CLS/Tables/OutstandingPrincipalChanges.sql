CREATE TABLE [pridrcrp].[OutstandingPrincipalChanges]
(
	[OutstandingPrincipalChangeId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerInformationId] INT NOT NULL,
	[BorrowerActivityId] INT NOT NULL,
	[OutstandingPrincipal] DECIMAL(14, 2),
	[EffectiveDate] DATE NULL,
	[InactivatedAt] DATE NULL,
	[InactivatedBy] VARCHAR(200) NULL,
	CONSTRAINT [FK_BorrowerInformation_OutstandingPrincipalChanges] FOREIGN KEY ([BorrowerInformationId]) REFERENCES pridrcrp.BorrowerInformation([BorrowerInformationId]),
	CONSTRAINT [FK_BorrowerActivityHistory_OutstandingPrincipalChanges] FOREIGN KEY ([BorrowerActivityId]) REFERENCES pridrcrp.BorrowerActivityHistory([BorrowerActivityId])
)
