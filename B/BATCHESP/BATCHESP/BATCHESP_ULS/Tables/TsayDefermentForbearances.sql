CREATE TABLE batchesp.TsayDefermentForbearances
(
	TsayDefermentForbearanceId	INT PRIMARY KEY IDENTITY(1,1) NOT NULL
	,BorrowerSSN				CHAR(9) NOT NULL
	,LoanSequence				SMALLINT NULL
	,[Type]						VARCHAR(9) NULL
	,BeginDate					DATE NULL
	,EndDate					DATE NULL
	,CertificationDate			DATE NULL
	,DeferSchool				VARCHAR(8) NULL
	,CreatedAt					DATETIME DEFAULT GETDATE() NOT NULL
	,CreatedBy					VARCHAR(50) DEFAULT USER_NAME() NOT NULL
	,ProcessedAt				DATETIME NULL
	,ProcessedBy				VARCHAR(50) NULL
	,UpdatedAt					DATETIME NULL
	,RequestedBeginDate			DATE NULL
	,RequestedEndDate			DATE NULL
);

