/*
   Wednesday, July 17, 201911:44:44 AM
   User: 
   Server: opsdev
   Database: CLS
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
USE CLS
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
ALTER TABLE dasforbfed.Disasters
	DROP CONSTRAINT DF__tmp_ms_xx__Added__6F8121F9
GO
ALTER TABLE dasforbfed.Disasters
	DROP CONSTRAINT DF__tmp_ms_xx__Added__70754632
GO
CREATE TABLE dasforbfed.Tmp_Disasters
	(
	DisasterId int NOT NULL IDENTITY (1, 1),
	Disaster varchar(100) NOT NULL,
	BeginDate date NOT NULL,
	EndDate date NOT NULL,
	Days  AS (datediff(day,[BeginDate],[EndDate])+(1)),
	MaxEndDate date NOT NULL,
	MaxDays  AS (datediff(day,[BeginDate],[MaxEndDate])+(1)),
	Active bit NOT NULL,
	AddedAt datetime NOT NULL,
	AddedBy varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dasforbfed.Tmp_Disasters SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dasforbfed.Tmp_Disasters ADD CONSTRAINT
	DF__tmp_ms_xx__Added__6F8121F9 DEFAULT (getdate()) FOR AddedAt
GO
ALTER TABLE dasforbfed.Tmp_Disasters ADD CONSTRAINT
	DF__tmp_ms_xx__Added__70754632 DEFAULT (suser_sname()) FOR AddedBy
GO
SET IDENTITY_INSERT dasforbfed.Tmp_Disasters ON
GO
IF EXISTS(SELECT * FROM dasforbfed.Disasters)
	 EXEC('INSERT INTO dasforbfed.Tmp_Disasters (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedAt, AddedBy)
		SELECT DisasterId, Disaster, CONVERT(date, BeginDate), CONVERT(date, EndDate), CONVERT(date, MaxEndDate), Active, AddedAt, AddedBy FROM dasforbfed.Disasters WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dasforbfed.Tmp_Disasters OFF
GO
ALTER TABLE dasforbfed.ProcessQueue
	DROP CONSTRAINT FK_ProcessQueue_Disasters
GO
ALTER TABLE dasforbfed.Zips
	DROP CONSTRAINT FK_Zips_Disasters
GO
DROP TABLE dasforbfed.Disasters
GO
EXECUTE sp_rename N'dasforbfed.Tmp_Disasters', N'Disasters', 'OBJECT' 
GO
ALTER TABLE dasforbfed.Disasters ADD CONSTRAINT
	PK__tmp_ms_x__B487740E283B4DE6 PRIMARY KEY CLUSTERED 
	(
	DisasterId
	) WITH( PAD_INDEX = OFF, FILLFACTOR = 95, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dasforbfed.Disasters
ALTER COLUMN [Days]
ADD PERSISTED;
GO
ALTER TABLE dasforbfed.Disasters
ALTER COLUMN [MaxDays]
ADD PERSISTED;
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dasforbfed.Zips ADD CONSTRAINT
	FK_Zips_Disasters FOREIGN KEY
	(
	DisasterId
	) REFERENCES dasforbfed.Disasters
	(
	DisasterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dasforbfed.Zips SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dasforbfed.ProcessQueue ADD CONSTRAINT
	FK_ProcessQueue_Disasters FOREIGN KEY
	(
	DisasterId
	) REFERENCES dasforbfed.Disasters
	(
	DisasterId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dasforbfed.ProcessQueue SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
