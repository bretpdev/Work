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

--TODO: Default EntityTypes ex. Forms, Users, Custom
--TODO: Default AttributeDataTypes ex. string, int, ssn, account number, etc.

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Entities_Entities_CreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[Entities]'))
ALTER TABLE [dbo].[Entities] DROP CONSTRAINT [FK_Entities_Entities_CreatedBy]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntityTypes_Entities_CreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntityTypes]'))
ALTER TABLE [dbo].[EntityTypes] DROP CONSTRAINT [FK_EntityTypes_Entities_CreatedBy]
GO

DELETE FROM EntityTypes
GO

DELETE FROM Entities
GO

INSERT INTO EntityTypes(EntityTypeDescription, CreatedBy)
VALUES('Employee', 0)
,('Forms', 0)
GO

SET IDENTITY_INSERT Entities ON
INSERT INTO Entities(EntityId, EntityName, EntityTypeId, CreatedBy)
VALUES(0, 'DCRUser', 1, 0)
SET IDENTITY_INSERT Entities OFF
GO

--Recreate the check constraints
--ALTER TABLE [dbo].[Entities]  WITH CHECK ADD  CONSTRAINT [FK_Entities_Entities_CreatedBy] FOREIGN KEY([CreatedBy])
--REFERENCES [dbo].[Entities] ([EntityId])
--GO

--ALTER TABLE [dbo].[Entities] CHECK CONSTRAINT [FK_Entities_Entities_CreatedBy]
--GO


ALTER TABLE [dbo].[EntityTypes]  WITH CHECK ADD  CONSTRAINT [FK_EntityTypes_Entities_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Entities] ([EntityId])
GO

ALTER TABLE [dbo].[EntityTypes] CHECK CONSTRAINT [FK_EntityTypes_Entities_CreatedBy]
GO


INSERT INTO AttributeDataTypes(AttributeDataTypeDescription, CreatedBy)
VALUES('SSN',  0)
,('Account Number', 0)
,('Text', 0)
,('Whole Number', 0)
,('True/False', 0)
,('Decimal',0)
,('USD',0)
,('Date',0)
,('Time',0)
,('DateTime',0)
,('EntityId',0)
,('ValueId',0)
GO

INSERT INTO AttributeSelectionTypes(AttributeSelectionTypeDescription, CreatedBy)
VALUES('Single', 0),
('Multiple Choice', 0),
('Multiple Answer', 0)
GO

INSERT INTO Attributes(AttributeDescription,AttributeLongDescription, AttributeDataTypeId, AttributeSelectionTypeId, CreatedBy)
VALUES('First Name','', 3, 1, 0),
('Middle Initial','', 3, 1, 0),
('Last Name','', 3, 1, 0),
('Age','', 4, 1, 0),
('Windows User Name','',3, 1,0)
GO

INSERT INTO HistoryStatusTypes([HistoryStatusTypeDescription])
VALUES('UPDATE'),
('INSERT'),
('DELETE')

--ADD ADDITIONAL POST DEPLOYMENT SCRIPTS HERE. DO NOT REMOVE
:r AddDevelopersAsEntities.sql