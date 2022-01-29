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

--DELETE FROM UpdateTypes

--INSERT INTO 
--	UpdateTypes (UpdateTypeId, AvatierCode, [Description])
--VALUES
--	(1, 'PROVISION', 'New Hire'),
--	(2, 'TERMINATION', 'Termination'),
--	(3, 'RENAME', 'Name Change'),
--	(4, 'TRANSFER', 'Role Change'),
--	(5, 'PROPERTY PUSH', 'Change Event')