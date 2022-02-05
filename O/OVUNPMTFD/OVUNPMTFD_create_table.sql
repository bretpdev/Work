USE CLS;
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE SCHEMA ovunpmtfd;
GO

CREATE TABLE ovunpmtfd.States
(
	StateId		INT IDENTITY(1,1) NOT NULL,
	[Name]		VARCHAR(50) NOT NULL,
	Abbreviation VARCHAR(2) NOT NULL,
	Active		BIT NOT NULL,
	AddedAt		DATE NOT NULL,
	AddedBy		VARCHAR(50) NOT NULL,
	DeletedAt	DATETIME NULL,
	DeletedBy	VARCHAR(50) NULL,
	CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
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

INSERT INTO ovunpmtfd.States
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
	'Colorado',
	'CO',
	@ACTIVE,
	@TODAY,
	SYSTEM_USER,
	NULL,
	NULL
),
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
	'Rhode Island',
	'RI',
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
)
;

----TEST:
--drop table ovunpmtfd.States
--drop schema ovunpmtfd