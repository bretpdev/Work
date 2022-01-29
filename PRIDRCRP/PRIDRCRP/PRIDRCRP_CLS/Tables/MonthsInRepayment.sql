CREATE TABLE [pridrcrp].[MonthsInRepayment]
(
	[MonthsInRepaymentId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerInformationId] INT NOT NULL,
	[AmountDue] DECIMAL(14,2) NULL,
	[AmountDueAdjusted] DECIMAL(14,2) NULL,
	[Date] DATE NULL,
	[RepaymentPlanTypeId] INT NULL,
	[CoveredByEHD] BIT NOT NULL DEFAULT(0),
	[CoveredByDefFor] BIT NOT NULL DEFAULT(0),
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(50),
	CONSTRAINT [FK_BorrowerInformation_MonthsInRepayment] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId]),
	CONSTRAINT [FK_RepaymentPlanTypes_MonthsInRepayment] FOREIGN KEY ([RepaymentPlanTypeId]) REFERENCES [pridrcrp].[RepaymentPlanTypes] ([RepaymentPlanTypeId])
)
