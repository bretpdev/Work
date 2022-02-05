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
GRANT EXECUTE ON GetBUIdsForPopulatedQueues TO db_executor
go

GRANT EXECUTE ON spQSTA_AddQueueData TO db_executor
go

GRANT EXECUTE ON spQSTA_AddUserData TO db_executor
go

GRANT EXECUTE ON spQSTA_GetDepartments TO db_executor
go

GRANT EXECUTE ON spQSTA_GetErrorBuMissing TO db_executor
go

GRANT EXECUTE ON spQSTA_GetErrorNoPopulate TO db_executor
go

GRANT EXECUTE ON spQSTA_GetErrorNoQueueDetail TO db_executor
go

GRANT EXECUTE ON spQSTA_GetReportData TO db_executor
go

