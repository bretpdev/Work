CREATE TABLE achrirdf.ProcessQueue
(
	ProcessQueueId INT IDENTITY(1,1) NOT NULL,
	Report VARCHAR(3) NOT NULL,
	Ssn CHAR(9) NOT NULL,
	AccountNumber CHAR(10) NOT NULL,
	LoanSequence VARCHAR(3) NOT NULL,
	OwnerCode VARCHAR(8) NOT NULL,
	DefermentOrForbearanceOriginalBeginDate DATE NULL,
	DefermentOrForbearanceOriginalEndDate DATE NULL,
	DefermentOrForbearanceBeginDate DATE NULL,
	DefermentOrForbearanceEndDate DATE NULL,
	UpdatedAt DATETIME NOT NULL,
	VariableRate BIT NOT NULL,
	HasPartialReducedRate BIT NULL,
	ProcessedAt DATETIME NULL, 
	ProcessedBy VARCHAR(50) NULL, 
	CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
	CreatedBy VARCHAR(50) DEFAULT SUSER_NAME() NOT NULL,
	PRIMARY KEY CLUSTERED ([ProcessQueueId] ASC)
);
