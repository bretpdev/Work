/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE BSYS
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'RTML_GetBarcodeInfoToInvalidate')
BEGIN
	DROP PROCEDURE RTML_GetBarcodeInfoToInvalidate
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'RTML_MarkBarcodeRecordCompleted')
BEGIN
	DROP PROCEDURE RTML_MarkBarcodeRecordCompleted
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'RTML_SaveScannedInfo')
BEGIN
	DROP PROCEDURE RTML_SaveScannedInfo
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RTML_DAT_BarcodeData')
	BEGIN
		DROP TABLE BSYS.dbo.RTML_DAT_BarcodeData
	END


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RTML_LST_ReturnReasons')
	BEGIN
		DROP TABLE BSYS.dbo.RTML_LST_ReturnReasons
	END


GRANT EXECUTE ON SCHEMA::bcsretmail TO db_executor