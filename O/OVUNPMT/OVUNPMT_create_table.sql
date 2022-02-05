USE [ULS];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE SCHEMA ovunpmt;
GO

CREATE TABLE ovunpmt.States
(
	StateId		INT IDENTITY(1,1) NOT NULL,
	[Name]		VARCHAR(50) NOT NULL,
	Abbreviation VARCHAR(2) NOT NULL,
	Active		BIT NOT NULL,
	AddedAt		DATE NOT NULL,
	AddedBy		VARCHAR(50) NOT NULL,
	DeletedAt	DATETIME NULL,
	DeletedBy	VARCHAR(50) NULL,
	CONSTRAINT PK_States PRIMARY KEY CLUSTERED 
(
	StateId ASC
) WITH (
			PAD_INDEX = OFF, 
			STATISTICS_NORECOMPUTE = OFF, 
			IGNORE_DUP_KEY = OFF, 
			ALLOW_ROW_LOCKS = ON, 
			ALLOW_PAGE_LOCKS = ON, 
			FILLFACTOR = 95
		) ON [PRIMARY]
) ON [PRIMARY]
;

GO

DECLARE @TODAY DATE = GETDATE(),
		@ACTIVE BIT = 1;

INSERT INTO ovunpmt.States
(
	[Name],
	Abbreviation,
	Active,
	AddedAt,
	AddedBy,
	DeletedAt,
	DeletedBy
)
VALUES
(
	'California',
	'CA',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'Colorado',
	'CO',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'Maine',
	'ME',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'New Jersey',
	'NJ',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'New York',
	'NY',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'Rhode Island',
	'RI',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
(
	'Virginia',
	'VA',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
)
;

----TEST:
--select * from uls.ovunpmt.States
--drop table ovunpmtfd.States
--drop schema ovunpmtfd
