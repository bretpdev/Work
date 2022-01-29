CREATE TABLE batchesp.Ts01Enrollments
(
	 Ts01EnrollmentId	INT PRIMARY KEY IDENTITY(1,1) NOT NULL
	,BorrowerSsn		CHAR(9) NOT NULL
	,LoanSequence		SMALLINT NULL
	,StudentSsn			CHAR(9)	NULL
	,SeparationDate		DATE NULL
	,SchoolCode			VARCHAR(8) NULL
	,SeparationReason	VARCHAR(2) NULL
	,SeparationSource	VARCHAR(2) NULL
	,DateNotified		DATE NULL
	,DateCertified		DATE NULL
	,CreatedAt			DATETIME DEFAULT GETDATE() NOT NULL
	,CreatedBy			VARCHAR(50) DEFAULT SYSTEM_USER NOT NULL
	,ProcessedAt		DATETIME NULL
	,ProcessedBy		VARCHAR(50) NULL
	,UpdatedAt			DATETIME NULL

);
