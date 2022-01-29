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
DELETE FROM trdprtyres.Relationships
DBCC CHECKIDENT('ULS.trdprtyres.Relationships', RESEED, 0)
DELETE FROM trdprtyres.Sources
DBCC CHECKIDENT('ULS.trdprtyres.Sources', RESEED, 0)

INSERT INTO trdprtyres.Relationships(Relationship, RelationshipCode, IsOnelink)
VALUES('EMPLOYER','EM', 1),
('FRIEND','FR', 1),
('GUARDIAN','GU', 1),
('NEIGHBOR','NE', 1),
('NOT AVAILABLE','N', 1),
('OTHER','OT', 1),
('PARENT','PA', 1),
('RELATIVE','RE', 1),
('ROOMMATE','RM', 1),
('SIBLING','SI', 1),
('SPOUSE','SP', 1),
('ATTORNEY','15', 0),
('CHILD','10', 0),
('EMPLOYER','05', 0),
('FRIEND','11', 0),
('GUARDIAN', '12', 0),
('NEIGHBOR','09', 0),
('NON-RELATIVE','04', 0),
('PARENT','02', 0),
('PHYSICIAN','13', 0),
('POA','16', 0),
('RELATIVE','03', 0),
('ROOM MATE','08', 0),
('SIBLING','07', 0),
('SPOUSE','06', 0),
('SURVIVOR','14', 0),
('UNKNOWN','01', 0)


INSERT INTO trdprtyres.Sources([Source], SourceCode, IsOnelink)
VALUES('Application','A', 1),
('Exit Interview','E', 1),
('Entrance Interview','N', 1),
('Forbearance','S', 1),
('Lender/Servicer','S', 1),
('Other','O', 1),
('Preclaim','U', 1),
('Repayment Agreement','R', 1),
('Web','W', 1),
('Application','02', 0),
('Attorney','31', 0),
('Child','22', 0),
('Correspondence','05', 0),
('Entrance Interview','32', 0),
('Exit Interview','32', 0),
('Forbearance','09', 0),
('Guardian','31', 0),
('Lender/Servicer','32', 0),
('Neighbor','16', 0),
('Non-Relative/Friend','23', 0),
('Other','31', 0),
('Parent','12', 0),
('Physician','31', 0),
('Preclaim','98', 0),
('Relative','31', 0),
('Repayment Agreement','06', 0),
('Room Mate','15', 0),
('Sibling','14', 0),
('Spouse','13', 0)

GRANT EXECUTE ON SCHEMA:: trdprtyres TO db_executor