/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if DATABASE_PRINCIPAL_ID('db_executor') is null
begin
	create role db_executor
end
grant execute to db_executor

if DATABASE_PRINCIPAL_ID('Uheaa\UheaaUsers') is null
begin
	create user [Uheaa\UheaaUsers] for login [Uheaa\UheaaUsers]
end

if DATABASE_PRINCIPAL_ID('Uheaa\CornerStoneUsers') is null
begin
	create user [Uheaa\CornerStoneUsers] for login [Uheaa\CornerStoneUsers]
end

if DATABASE_PRINCIPAL_ID('Uheaa\SystemAnalysts') is null
begin
	create user [Uheaa\SystemAnalysts] for login [Uheaa\SystemAnalysts]
end

if DATABASE_PRINCIPAL_ID('Uheaa\Developers') is null
begin
	create user [Uheaa\Developers] for login [Uheaa\Developers]
end

exec sp_addrolemember 'db_executor', 'Uheaa\UheaaUsers'
exec sp_addrolemember 'db_datareader', 'Uheaa\UheaaUsers'

exec sp_addrolemember 'db_executor', 'Uheaa\CornerStoneUsers'
exec sp_addrolemember 'db_datareader', 'Uheaa\CornerStoneUsers'

exec sp_addrolemember 'db_executor', 'Uheaa\SystemAnalysts'
exec sp_addrolemember 'db_datareader', 'Uheaa\SystemAnalysts'

exec sp_addrolemember 'db_executor', 'Uheaa\Developers'
exec sp_addrolemember 'db_datareader', 'Uheaa\Developers'

GO
USE CLS
GO
IF NOT EXISTS (SELECT NULL FROM sys.schemas WHERE name = 'log')
BEGIN
    EXEC ('CREATE SCHEMA [log] AUTHORIZATION [dbo]')
END
GO

USE ULS
GO
IF NOT EXISTS (SELECT NULL FROM sys.schemas WHERE name = 'log')
BEGIN
    EXEC ('CREATE SCHEMA [log] AUTHORIZATION [dbo]')
END
GO
USE ProcessLogs
GO
