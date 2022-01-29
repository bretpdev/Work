/*
   Tuesday, July 03, 20188:37:59 AM
   User: 
   Server: opsdev
   Database: UDW
   Application: 
*/
USE UDW
GO

DELETE FROM UDW..PD10_PRS_NME WHERE DF_PRS_ID = '060788644' --Remove bad ssn from live

GO
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
ALTER TABLE dbo.GR10_RPT_LON_APL ADD
	LF_FED_CLC_RSK varchar(6) NULL
GO
ALTER TABLE dbo.GR10_RPT_LON_APL SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
