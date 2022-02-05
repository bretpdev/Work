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

CREATE TABLE progrevw.GENR_REF_LoanTypes 
(
	LoanTypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	LoanType VARCHAR(25) NOT NULL,
	LoanProgram VARCHAR(50) NOT NULL
)
GO

--populate with data from Duster/sas/whse/progrevw/GENR_REF_LoanTypes.txt
INSERT INTO progrevw.GENR_REF_LoanTypes
(
	LoanType,
	LoanProgram
)
VALUES
	('SF','FFEL'),
	('SU','FFEL'),
	('GB','FFEL'),
	('PLUS','FFEL'),
	('PLUSGB','FFEL'),
	('CL','FFEL'),
	('CNSLDN','FFEL'),
	('SUBCNS','FFEL'),
	('UNCNS','FFEL'),
	('SUBSPC','FFEL'),
	('UNSPC','FFEL'),
	('STFFRD','FFEL'),
	('UNSTFD','FFEL'),
	('PL','FFEL'),
	('SLS','FFEL'),
	('GOMD','PRIVATE'),
	('MD','PRIVATE'),
	('GOPA','PRIVATE'),
	('PA','PRIVATE'),
	('GOPT','PRIVATE'),
	('GORX','PRIVATE'),
	('PT','PRIVATE'),
	('RX','PRIVATE'),
	('GORN','PRIVATE'),
	('RN','PRIVATE'),
	('GATEUG','PRIVATE'),
	('GU','PRIVATE'),
	('GATEGL','PRIVATE'),
	('GG','PRIVATE'),
	('GATEMD','PRIVATE'),
	('GM','PRIVATE'),
	('GOED','PRIVATE'),
	('GOHADM','PRIVATE'),
	('GOTHPY','PRIVATE'),
	('GOPADM','PRIVATE'),
	('GOPUBH','PRIVATE'),
	('GOSOCL','PRIVATE'),
	('GOSTAT','PRIVATE'),
	('GOENG','PRIVATE'),
	('GOPHD','PRIVATE'),
	('LAW','PRIVATE'),
	('MBA','PRIVATE'),
	('EDUCATION','PRIVATE'),
	('HADM','PRIVATE'),
	('THPY','PRIVATE'),
	('PADM','PRIVATE'),
	('PUBH','PRIVATE'),
	('SOCL','PRIVATE'),
	('STATS','PRIVATE'),
	('ENGINEER','PRIVATE'),
	('PHD','PRIVATE'),
	('COMPLT','PRIVATE'),
	('SL','FFEL')
;