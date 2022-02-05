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
IF OBJECT_ID('webapi.ApiTokenControllerAccess', 'U') IS NOT NULL 
	DROP TABLE webapi.ApiTokenControllerAccess;
IF OBJECT_ID('webapi.RoleControllerAccess', 'U') IS NOT NULL 
	DROP TABLE webapi.RoleControllerAccess;
IF OBJECT_ID('webapi.ControllerRegions', 'U') IS NOT NULL 
	DROP TABLE webapi.ControllerRegions;

--MERGE webapi.Controllers AS t USING
--(
--	SELECT 1 [ControllerId], 'Borrower' [Name]
--	UNION
--	SELECT 2, 'NORAD DB'
--	UNION
--	SELECT 3, 'CSYS DB'
--	UNION
--	SELECT 4, 'CDW DB'
--	UNION
--	SELECT 5, 'CLS DB'
--	UNION
--	SELECT 6, 'UDW DB'
--	UNION
--	SELECT 7, 'ULS DB'
--	UNION
--	SELECT 8, 'Noble'
--) AS s
--ON t.ControllerId = s.ControllerId
--WHEN NOT MATCHED BY target THEN
--	INSERT ([ControllerId], [Name])
--	VALUES ([ControllerId], [Name])
--WHEN NOT MATCHED BY source THEN
--	DELETE
--WHEN MATCHED THEN
--	UPDATE SET [Name] = s.[Name]
--;

--MERGE webapi.ControllerRegions AS t USING
--(
--	SELECT 1 [ControllerRegionId], 'UHEAA' [Name]
--	UNION
--	SELECT 2, 'CornerStone'
--) AS s
--ON t.ControllerRegionId = s.ControllerRegionId
--WHEN NOT MATCHED BY target THEN
--	INSERT ([ControllerRegionId], [Name])
--	VALUES ([ControllerRegionId], [Name])
--WHEN NOT MATCHED BY source THEN
--	DELETE
--WHEN MATCHED THEN
--	UPDATE SET [Name] = s.[Name]
--;

--MERGE webapps.Groups AS t USING
--(
--	SELECT 1 [GroupId], 'UHEAA Developers' [ActiveDirectoryGroupName]
--	UNION
--	SELECT 2, 'ROLE - Systems Support - Business Systems Specialist'
--) AS s
--ON t.GroupId = s.GroupId
--WHEN NOT MATCHED BY target THEN
--	INSERT ([GroupId], [ActiveDirectoryGroupName])
--	VALUES ([GroupId], [ActiveDirectoryGroupName])
--WHEN NOT MATCHED BY source THEN
--	DELETE
--WHEN MATCHED THEN
--	UPDATE SET [ActiveDirectoryGroupName] = s.[ActiveDirectoryGroupName]
--;