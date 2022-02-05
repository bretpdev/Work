CREATE TABLE [pridrcrp].[BorrowerInformation]
(
	[BorrowerInformationId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Ssn] VARCHAR(9) NOT NULL,
	[InterestRate] DECIMAL(14, 3) NOT NULL,
	[FirstPayDue] DATE NOT NULL,
	[PaymentAmount] DECIMAL(14, 2) NOT NULL,
	[RepayPlan] VARCHAR(50) NOT NULL,
	[Page] CHAR(1) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL,
	[ZipFile] VARCHAR(200) NULL
) 
