USE CLS
GO
/*
   Wednesday, August XX, XXXXX:XX:XX AM
   User: 
   Server: uheaasqldb
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
ALTER TABLE dbo.CheckByPhone
	DROP CONSTRAINT DF_CheckByPhone_DataSource
GO
ALTER TABLE dbo.CheckByPhone ADD CONSTRAINT
	DF_CheckByPhone_DataSource DEFAULT ('UNKNOWN') FOR DataSource
GO
ALTER TABLE dbo.CheckByPhone SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
