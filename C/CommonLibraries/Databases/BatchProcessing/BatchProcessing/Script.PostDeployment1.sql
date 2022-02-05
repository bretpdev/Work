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
GRANT EXECUTE ON AddLoginType TO db_executor
GO

GRANT EXECUTE ON AddHistoryRecord TO db_executor
GO

GRANT EXECUTE ON GetAllLogins TO db_executor
GO

GRANT EXECUTE ON GetAllLoginTypes TO db_executor
GO

GRANT EXECUTE ON GetAllUsers TO db_executor
GO

GRANT EXECUTE ON GetNextAvailableBatchId TO db_executor
GO

GRANT EXECUTE ON GetRelatedScriptsForLoginId TO db_executor
GO

GRANT EXECUTE ON InsertLoginScriptTracking TO db_executor
GO

GRANT EXECUTE ON spAddUserIdAndPassword TO db_executor
GO

GRANT EXECUTE ON spBLDBGetUserIdsAndPasswords TO db_executor
GO

GRANT EXECUTE ON spDeleteUserIdsAndPasswords TO db_executor
GO
GRANT EXECUTE ON spGetDecrpytedPassword TO db_executor
GO
GRANT EXECUTE ON spUpdateUserIdsAndPasswords TO db_executor
GO

GRANT EXECUTE ON UpdateLoginType TO db_executor
GO

GRANT EXECUTE ON GetLoginTypeId TO db_executor
GO

