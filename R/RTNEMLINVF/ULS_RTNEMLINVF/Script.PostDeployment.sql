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

TRUNCATE TABLE rtnemlinvf.EmaillAddresses

INSERT INTO rtnemlinvf.EmaillAddresses(EmailAddress, AddedAt, AddedBy)
VALUES('noreply@utahsbr.edu', GETDATE(), SUSER_SNAME())
,('servicemembers@utahsbr.edu', GETDATE(), SUSER_SNAME())
,('uheaa@utahsbr.edu', GETDATE(), SUSER_SNAME())