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

INSERT INTO Entities(EntityName, EntityTypeId, CreatedBy)
VALUES('Bret Pehrson', 1, 0),
('Eric Barnes', 1, 0),
('Jarom Ryan', 1, 0),
('Evan Walker', 1, 0),
('Jay Davis', 1, 0),
('Scott Briggs', 1, 0)
GO

INSERT INTO [Values] (StringValue)
VALUES
('Eric'),
('Barnes'),
('39'),
('M' ),
('Bret' ),
('Pehrson'),
('Jarom' ),
('Ryan'),
('jryan'), --9
('bpehrson'),
('ebarnes'),
('ewalker'),
('jdavis')


INSERT INTO EntityAttributeValues(EntityId, AttributeId, ValueId, CreatedBy)
VALUES
(2, 1, 1, 0),
(2, 2, 4, 0),
(2, 3, 2, 0),
(2, 4, 3, 0),
(1, 1, 5, 0),
(1, 3, 6, 0),
(1, 4, 3, 0),
(3, 5, 9, 0),
(1, 5, 10, 0),
(2, 5, 11, 0),
(4, 5, 12, 0),
(5, 5, 13, 0)


INSERT INTO EntityTypeAttributes([EntityTypeId],[AttributeId],[CreatedBy])
VALUES
(1,1,0),
(1,2,0),
(1,3,0),
(1,4,0),
(1,5,0)
