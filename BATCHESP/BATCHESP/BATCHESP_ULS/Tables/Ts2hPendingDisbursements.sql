CREATE TABLE batchesp.Ts2hPendingDisbursements
(
	Ts2hPendingDisbursementId			INT PRIMARY KEY IDENTITY(1,1) NOT NULL
	,BorrowerSsn	CHAR(9) NOT NULL
	,LoanSequence	SMALLINT NULL
	,DisbSequence	SMALLINT NULL
	,DisbType		VARCHAR(11) NULL
	,CreatedAt		DATETIME DEFAULT GETDATE() NOT NULL
	,CreatedBy		VARCHAR(50) DEFAULT USER_NAME() NOT NULL
	,ProcessedAt				DATETIME NULL
	,ProcessedBy				VARCHAR(50) NULL
	,UpdatedAt					DATETIME NULL 
    ,DisbursementDate DATE NULL

);
