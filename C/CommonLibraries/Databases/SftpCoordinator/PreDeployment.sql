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

if DATABASE_PRINCIPAL_ID('Uheaa\Batchscripts') is null
begin
	create user [Uheaa\Batchscripts] for login [Uheaa\Batchscripts]
end
if DATABASE_PRINCIPAL_ID('Uheaa\UheaaCrypt') is null
begin
	create user [Uheaa\UheaaCrypt] for login [Uheaa\UheaaCrypt]
end
if DATABASE_PRINCIPAL_ID('Uheaa\Developers') is null
begin
	create user [Uheaa\Developers] for login [Uheaa\Developers]
end


exec sp_addrolemember 'db_executor', 'Uheaa\Batchscripts'
exec sp_addrolemember 'db_executor', 'Uheaa\UheaaCrypt'
exec sp_addrolemember 'db_datareader', 'Uheaa\Developers'