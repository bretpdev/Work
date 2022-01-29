/*
   Friday, August 16, 20192:38:48 PM
   User: 
   Server: opsdev
   Database: CLS
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
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
ALTER TABLE scra.DataComparison ADD
	ServiceComponent char(2) NULL,
	EIDServiceComponent char(2) NULL,
	EndorserServiceComponent char(2) NULL,
	EndorserEIDServiceComponent char(2) NULL,
	ActiveBeginBrwr date NULL,
	ActiveEndBrwr date NULL,
	ActiveBeginEndr date NULL,
	ActiveEndEndr date NULL
GO
ALTER TABLE scra.DataComparison SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
