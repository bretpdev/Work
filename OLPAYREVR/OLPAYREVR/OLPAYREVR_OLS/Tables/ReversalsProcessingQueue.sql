CREATE TABLE [olpayrevr].[ReversalsProcessingQueue]
(
	[ProcessingQueueId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Ssn] CHAR(9) NOT NULL,
	[PaymentAmount] FLOAT NOT NULL,
	[PaymentEffectiveDate] DATE NOT NULL,
	[PaymentPostDate] DATE NULL,
	[PaymentType] CHAR(2) NOT NULL,
	[PaymentAlreadyReversed] BIT NULL,
	[HadError] BIT NULL,
	[ErrorDescription] VARCHAR(500) NULL,
	[ProcessedAt] DATETIME NULL,
	[CreatedAt] DATETIME NOT NULL,
	[CreatedBy] VARCHAR(25) NOT NULL DEFAULT SUSER_SNAME(), 
	[DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(25) NULL
)
