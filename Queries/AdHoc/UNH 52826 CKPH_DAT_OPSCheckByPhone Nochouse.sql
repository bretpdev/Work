USE NORAD
GO
/*
   Wednesday, August 16, 20179:31:46 AM
   User: 
   Server: nochouse
   Database: NORAD
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
ALTER TABLE dbo.CKPH_DAT_OPSCheckByPhone
	DROP CONSTRAINT DF_CKPH_DAT_OPSCheckByPhone_DataSource
GO
ALTER TABLE dbo.CKPH_DAT_OPSCheckByPhone ADD CONSTRAINT
	DF_CKPH_DAT_OPSCheckByPhone_DataSource DEFAULT ('UNKNOWN') FOR DataSource
GO
ALTER TABLE dbo.CKPH_DAT_OPSCheckByPhone SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
