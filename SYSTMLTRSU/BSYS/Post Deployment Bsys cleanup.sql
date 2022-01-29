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
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_US13B9CSG3_FormFields' WHERE StoredProcedureName = 'LT_US13B9CSG3_Fields'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_US06BNDSKP_FormFields' WHERE StoredProcedureName = 'LT_US06BNDSKP_BorrowerDemo'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_US06BEFSKP_FormFields' WHERE StoredProcedureName = 'LT_US06BEFSKP_BorrowerDemos'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_US06BFRPA_FormFields' WHERE StoredProcedureName = 'LT_US06BFRPA_Fields'

DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE LetterId = 718 --US06BDDSMC
