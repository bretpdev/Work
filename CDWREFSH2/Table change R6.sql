/*
   Friday, April 24, 20208:17:55 AM
   User: 
   Server: OPSDEV
   Database: CDW
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
USE CDW
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE FsaInvMet.R6
	DROP CONSTRAINT DF__R6__CreatedAt__60DD3190
GO
ALTER TABLE FsaInvMet.R6
	DROP CONSTRAINT DF__R6__CreatedBy__61D155C9
GO
CREATE TABLE FsaInvMet.Tmp_R6
	(
	R6Id int NOT NULL IDENTITY (1, 1),
	ReportDate datetime2(7) NOT NULL,
	Cat01Count int NULL,
	Cat02Count int NULL,
	Cat03Count int NULL,
	Cat04Count int NULL,
	Cat05Count int NULL,
	Cat06Count int NULL,
	Cat07Count int NULL,
	Cat08Count int NULL,
	Cat09Count int NULL,
	Cat10Count int NULL,
	Cat11Count int NULL,
	Cat12Count int NULL,
	Cat13Count int NULL,
	TotalCount  AS (((((((((((isnull([Cat01Count],(0))+isnull([Cat02Count],(0)))+isnull([Cat03Count],(0)))+isnull([Cat04Count],(0)))+isnull([Cat05Count],(0)))+isnull([Cat06Count],(0)))+isnull([Cat07Count],(0)))+isnull([Cat08Count],(0)))+isnull([Cat09Count],(0)))+isnull([Cat10Count],(0)))+isnull([Cat11Count],(0)))+isnull([Cat12Count],(0))+isnull([Cat13Count],(0))) PERSISTED ,
	CreatedAt datetime2(7) NOT NULL,
	CreatedBy varchar(50) NOT NULL,
	DeletedAt datetime2(7) NULL,
	DeletedBy varchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE FsaInvMet.Tmp_R6 SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE FsaInvMet.Tmp_R6 ADD CONSTRAINT
	DF__R6__CreatedAt__60DD3190 DEFAULT (getdate()) FOR CreatedAt
GO
ALTER TABLE FsaInvMet.Tmp_R6 ADD CONSTRAINT
	DF__R6__CreatedBy__61D155C9 DEFAULT (suser_sname()) FOR CreatedBy
GO
SET IDENTITY_INSERT FsaInvMet.Tmp_R6 ON
GO
IF EXISTS(SELECT * FROM FsaInvMet.R6)
	 EXEC('INSERT INTO FsaInvMet.Tmp_R6 (R6Id, ReportDate, Cat01Count, Cat02Count, Cat03Count, Cat04Count, Cat05Count, Cat06Count, Cat07Count, Cat08Count, Cat09Count, Cat10Count, Cat11Count, Cat12Count, Cat13Count, CreatedAt, CreatedBy, DeletedAt, DeletedBy)
		SELECT R6Id, ReportDate, Cat01Count, Cat02Count, Cat03Count, Cat04Count, Cat05Count, Cat06Count, Cat07Count, Cat08Count, Cat09Count, Cat10Count, Cat11Count, Cat12Count, NULL, CreatedAt, CreatedBy, DeletedAt, DeletedBy FROM FsaInvMet.R6 WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT FsaInvMet.Tmp_R6 OFF
GO
DROP TABLE FsaInvMet.R6
GO
EXECUTE sp_rename N'FsaInvMet.Tmp_R6', N'R6', 'OBJECT' 
GO
ALTER TABLE FsaInvMet.R6 ADD CONSTRAINT
	PK__R6__49CA78304F37BF6B PRIMARY KEY CLUSTERED 
	(
	R6Id
	) WITH( PAD_INDEX = OFF, FILLFACTOR = 95, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
