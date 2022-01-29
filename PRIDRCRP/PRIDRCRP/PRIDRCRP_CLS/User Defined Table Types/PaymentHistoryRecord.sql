CREATE TYPE [pridrcrp].[PaymentHistoryRecord] AS TABLE
(
	[Description] VARCHAR(20) NOT NULL,
	[ActionDate] DATE NULL,
	[EffectiveDate] DATE NULL,
	[TotalPaid] DECIMAL(14,2) NULL,
	[InterestPaid] DECIMAL(14,2) NULL,
	[PrincipalPaid] DECIMAL(14,2) NULL,
	[LcNsfPaid] DECIMAL(14,2) NULL,
	[AccruedInterest] DECIMAL(14,2) NULL,
	[LcNsfDue] DECIMAL(14,2) NULL,
	[PrincipalBalance] DECIMAL(14,2) NULL
)
