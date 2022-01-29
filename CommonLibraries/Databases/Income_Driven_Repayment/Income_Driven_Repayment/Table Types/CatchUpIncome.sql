CREATE TYPE [dbo].[CatchUpIncome] AS TABLE
(
	RepayeEndDate DATETIME,
	CreateDate DATETIME,
	[Year] INT,
	AGI DECIMAL(18,2),
	[State] CHAR (2),
	FamilySize INT,
	[Source] VARCHAR(3),
	ExternalDebt DECIMAL(18,2)
)
