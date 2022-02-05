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
                 AND  TABLE_NAME = 'SYSA_Ivr'))
BEGIN
    DROP TABLE SYSA_Ivr
END

IF OBJECT_ID('CheckUsersRoleForIvr', 'P') IS NOT NULL
DROP PROC MySproc
GO

TRUNCATE TABLE [ccclosures].RegionAccess
DELETE FROM [ccclosures].IvrRoles
DBCC CHECKIDENT ('[ccclosures].IvrRoles', RESEED, 0)
DELETE FROM [ccclosures].Regions
DBCC CHECKIDENT ('[ccclosures].Regions', RESEED, 0)

INSERT INTO [ccclosures].IvrRoles(RoleId)
VALUES(1),
(5),
(6),
(9),
(10),
(11),
(39),
(40),
(42),
(44),
(71)

INSERT INTO [ccclosures].Regions(RegionName)
VALUES('CornerStone'),
('UHEAA LPP'),
('UHEAA LGP PRE'),
('UHEAA LGP DFT')

INSERT INTO [ccclosures].RegionAccess(RoleId, RegionsId)
VALUES(1, 1),
(1, 2),
(1, 3),
(1, 4),
(2, 1),
(2, 2),
(3, 1),
(3, 2),
(4, 1),
(4, 2),
(5, 1),
(5, 2),
(5, 3),
(5, 4),
(6, 1),
(6, 2),
(6, 3),
(6, 4),
(7, 3),
(7, 4),
(8, 3),
(8, 4),
(9, 3),
(9, 4),
(10, 3),
(10, 4),
(11, 1),
(11, 2)