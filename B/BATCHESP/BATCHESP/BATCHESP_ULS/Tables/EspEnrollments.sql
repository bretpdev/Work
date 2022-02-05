CREATE TABLE batchesp.EspEnrollments
(
	 EspEnrollmentId		INT PRIMARY KEY IDENTITY(1,1) NOT NULL
	,BorrowerSsn			CHAR(9) NOT NULL
	,AccountNumber			CHAR(10) NULL
	,[Queue]				VARCHAR(2) NULL
	,SubQueue				VARCHAR(2) NULL
	,TaskControlNumber		VARCHAR(18) NULL
	,Arc					VARCHAR(5) NULL
	,ArcRequestDate			DATE NULL
	,Message1				VARCHAR(77) NULL
	,SupplementalMessage	VARCHAR(77) NULL
	,StudentSSN				VARCHAR(9) NULL
	,StudentSSN2			VARCHAR(9) NULL
	,SchoolCode				VARCHAR(8) NULL
	,ESP_Status				VARCHAR(2) NULL
	,ESP_SeparationDate		DATE NULL
	,ESP_CertificationDate	DATE NULL
	,EnrollmentBeginDate	DATE NULL
	,SourceCode				VARCHAR(2) NULL
	,CreatedAt				DATETIME DEFAULT GETDATE() NOT NULL
	,CreatedBy				VARCHAR(50) DEFAULT USER_NAME() NOT NULL
	,ProcessedAt			DATETIME NULL
	,ProcessedBy			VARCHAR(50) NULL
		,UpdatedAt					DATETIME NULL, 
    [RequiresReview] BIT NULL

);
