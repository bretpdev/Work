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
USE ProcessLogs
GO

GRANT EXECUTE ON ExceptionLogInsert TO db_executor
GRANT EXECUTE ON ExceptionLogResolve TO db_executor
GRANT EXECUTE ON ProcessNotificationInsert TO db_executor
GRANT EXECUTE ON BusinessUnitsByScript TO db_executor
GRANT EXECUTE ON BusinessUnitByID TO db_executor
GRANT EXECUTE ON AddBusinessUnit TO db_executor
GRANT EXECUTE ON AddBusinessUnitByID TO db_executor
GRANT EXECUTE ON EndBusinessUnit TO db_executor
GRANT EXECUTE ON InsertProcessStart TO db_executor
GRANT EXECUTE ON EndScriptRun TO db_executor
GRANT EXECUTE ON [LogEndOfJobResults] TO db_executor

DELETE FROM NotificationTypes
GO
INSERT INTO NotificationTypes(NotificationTypeId ,NotificationTypeDescription)
VALUES(0,'No File')
,(1,'File Format Problem')
,(2,'Empty File')
,(3,'Error Report')
,(4,'Handled Exception')
,(5,'Other');
GO

DELETE FROM NotificationSeverityTypes
GO
INSERT INTO NotificationSeverityTypes(NotificationSeverityTypeId ,NotificationSeverityTypeDescription)
VALUES(0,'Informational')
,(1,'Warning')
,(2,'Critical');

GO
-- Additional Post Deployment scripts
:r "Stored Procedures\BSYS\BusinessUnitsByScript.sql"
:r "Tables\CLS\ProcessLogMessages.sql"
:r "Stored Procedures\CLS\NotificationInsert.sql"
:r "Tables\ULS\ProcessLogMessages.sql"
:r "Stored Procedures\ULS\NotificationInsert.sql"
