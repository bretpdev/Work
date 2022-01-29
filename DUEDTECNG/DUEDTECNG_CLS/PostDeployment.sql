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
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'DueDateChange'))
BEGIN
	BEGIN TRANSACTION

    INSERT INTO duedtecng.DueDateChange 
	([Ssn], [AccountNumber], [DueDate], [Arc], [Comment], [ProcessedAt], [ProcessedBy], [AddedAt], [AddedBy], [DeletedAt], [DeletedBy])
	SELECT
		[Ssn], [AccountNumber], [DueDate], [Arc], [Comment], [ProcessedAt], CASE WHEN [ProcessedAt] IS NULL THEN NULL ELSE 'IMPORT FROM DBO TO DUEDTECNG' END, 
		[AddedAt], 'IMPORT FROM DBO TO DUEDTECNG', CASE WHEN [Successful] = 1 THEN NULL ELSE [ProcessedAt] END, CASE WHEN [Successful] = 1 THEN NULL ELSE 'IMPORT FROM DBO TO DUEDTECNG' END
	FROM
		dbo.DueDateChange

	DROP TABLE dbo.DueDateChange

	IF @@ERROR = 0
		COMMIT TRANSACTION
	ELSE
		ROLLBACK TRANSACTION

END

IF EXISTS(SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.GetDueDateChangeRecordCount'))
	DROP PROCEDURE dbo.GetDueDateChangeRecordCount
IF EXISTS(SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.GetNextDueDateChangeRecord'))
	DROP PROCEDURE dbo.GetNextDueDateChangeRecord
IF EXISTS(SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.MarkDueDateAsProcessed'))
	DROP PROCEDURE dbo.MarkDueDateAsProcessed
IF EXISTS(SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.UpdateDueDateChangeSuccess'))
	DROP PROCEDURE dbo.UpdateDueDateChangeSuccess

IF NOT EXISTS (SELECT * FROM duedtecng.AppSettings)
	INSERT INTO duedtecng.AppSettings (AppSettingsId, MaxIncrease) VALUES (1, 25.00)