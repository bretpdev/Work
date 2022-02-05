CREATE TABLE [pridrcrp].[RepaymentPlanChanges]
(
	[RepaymentPlanChangeId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerInformationId] INT NOT NULL,
	[BorrowerActivityId] INT NULL,
	[PlanType] VARCHAR(MAX) NOT NULL,
	[EffectiveDate] DATE NULL,
	[InactivatedAt] DATE NULL,
	[InactivatedBy] VARCHAR(200) NULL,
	CONSTRAINT [FK_BorrowerInformation_RepaymentPlanChanges] FOREIGN KEY ([BorrowerInformationId]) REFERENCES pridrcrp.BorrowerInformation([BorrowerInformationId]),
	CONSTRAINT [FK_BorrowerActivityHistory_RepaymentPlanChanges] FOREIGN KEY ([BorrowerActivityId]) REFERENCES pridrcrp.BorrowerActivityHistory([BorrowerActivityId])
)
