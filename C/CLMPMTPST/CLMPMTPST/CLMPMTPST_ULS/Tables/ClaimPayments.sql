CREATE TABLE [clmpmtpst].[ClaimPayments]
(
	[ClaimPaymentId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AccountNumber] CHAR(10) NOT NULL,
	[PaymentAmount] NUMERIC(9,2) NOT NULL,
	[EffectiveDate] DATE NOT NULL,
	[LoanSequence] INT NOT NULL,
	[LastName] VARCHAR(23) NOT NULL,
	[BatchNumber] VARCHAR(30) NULL,
	[ProcessedAt] DATETIME NULL,
	[ProcessedManually] BIT NOT NULL DEFAULT 0,
	[ErrorId] INT NULL,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(100) NULL,
	CONSTRAINT [FK_ClaimPayments_Errors] FOREIGN KEY (ErrorId) REFERENCES clmpmtpst.Errors([ErrorId]),
)
