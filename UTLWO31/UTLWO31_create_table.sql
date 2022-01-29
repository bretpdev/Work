USE UDW;
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS
(
	SELECT
		*
	FROM
		sys.schemas
	WHERE
		NAME = N'progrevw'
)
EXEC ('CREATE SCHEMA progrevw AUTHORIZATION dbo');
GO

CREATE TABLE progrevw.LDR_AFF
(
	LDR_AFF_ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	LenderId VARCHAR(8) NOT NULL,
	Affiliation VARCHAR(25) NOT NULL
)
GO

--populate with data from Duster/sas/whse/progrevw/LDR_AFF
INSERT INTO progrevw.LDR_AFF
(
	LenderId,
	Affiliation
)
VALUES
	('828476','UHEAA'),
	('834396','UHEAA'),
	('834437','UHEAA'),
	('82847601','UHEAA'),
	('834437','CUSTODIAN'),
	('834493','CUSTODIAN'),
	('834493','UHEAA'),
	('834529','UHEAA'),
	('826717','UHEAA'),
	('830248','UHEAA'),
	('971357','UHEAA'),
	('900749','UHEAA')
;