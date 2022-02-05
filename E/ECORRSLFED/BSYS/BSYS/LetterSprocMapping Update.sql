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
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BD201_DefermentType'												   
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BD201_Deferment'												   
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BBAPP_Loans'												   
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BBCANC_Loans'												   
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BD10DC_Name'
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BDPLUS_Fields'
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE StoredProcedureName ='LT_TS06BRPY45_Deadline'
DELETE FROM BSYS..LTDB_SystemLettersStoredProcedures WHERE SystemLettersStoredProcedureId IN (370,374,382,383) --TS06BRPY45,TS06BR001 double mapped in live

UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BD101_Loans' WHERE StoredProcedureName = 'GetLetterData_TS06BD101'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BF101_Loans' WHERE StoredProcedureName = 'GetLetterData_TS06BF101'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BF101C_Loans' WHERE StoredProcedureName = 'GetLetterData_TS06BF101C'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BF101J_Loans' WHERE StoredProcedureName = 'GetLetterData_TS06BF101J'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BIBRXP_FormFields' WHERE StoredProcedureName = 'LT_TS06BIBRXP_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BIBR45_FormFields' WHERE StoredProcedureName = 'LT_TS06BIBR45_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BICR45_FormFields' WHERE StoredProcedureName = 'LT_TS06BICR45_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BICRE1_FormFields' WHERE StoredProcedureName = 'LT_TS06BICRE1_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BPYE45_FormFields' WHERE StoredProcedureName = 'LT_TS06BPYE45_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BPYEXP_FormFields' WHERE StoredProcedureName = 'LT_TS06BPYEXP_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BRPYXP_FormFields' WHERE StoredProcedureName = 'LT_TS06BRPYXP_IDR'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BRPY45_FormFields' WHERE StoredProcedureName = 'LT_TS06RPYE45_Deadline'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS11BISF_FormFields' WHERE StoredProcedureName = 'LT_TS11BISF_Reversed_Transaction'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BIDRPS_FormFields' WHERE StoredProcedureName = 'LT_TS06BIDRPS_Fields'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BSCRAE_FormFields' WHERE StoredProcedureName = 'LT_TS06BSCRAE_ExpirationDate'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BTM2C_FormFields' WHERE StoredProcedureName = 'LT_TS06BTM2C_ConsolidationPayment'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BTM2LM_FormFields' WHERE StoredProcedureName = 'LT_TS06BTM2LM'
UPDATE BSYS..LTDB_SystemLettersStoredProcedures SET StoredProcedureName = 'LT_TS06BIDRFG_Loans', ReturnTypeId = 2 WHERE StoredProcedureName = 'LT_TS06BIDRF_LOANS'

INSERT INTO BSYS..LTDB_SystemLettersStoredProcedures(LetterId, StoredProcedureName, ReturnTypeId, Active)
VALUES
	/*TS06BNEGAM*/
	(1463, 'LT_Header', 1, 1),
	(1463, 'LT_TS06BNEGAM_Loans', 2, 1),
	(1463, 'LT_TS06BNEGAM_FormFields', 3, 1)

USE CDW
GO

DROP PROCEDURE [dbo].[LT_TS06BD201_DefermentType]
DROP PROCEDURE [dbo].[LT_TS06BD201_Deferment]
DROP PROCEDURE [dbo].[LT_TS06BD201_Loans]
DROP PROCEDURE [dbo].[LT_TS06BD101_Deferments]
DROP PROCEDURE [dbo].[LT_TS06BBAPP_Loans]
DROP PROCEDURE [dbo].[LT_TS06BBCANC_Loans]
DROP PROCEDURE [dbo].[LT_TS06BD10DC_Name]
DROP PROCEDURE [dbo].[LT_TS06BD501_Loans]
DROP PROCEDURE [dbo].[LT_TS06BD601_Loans]
DROP PROCEDURE [dbo].[LT_TS06BD6016_Name]
DROP PROCEDURE [dbo].[LT_TS06BD6016_Loans]
DROP PROCEDURE [dbo].[LT_TS06BDD20_Name]
DROP PROCEDURE [dbo].[LT_TS06BDD80_Name]
DROP PROCEDURE [dbo].[LT_TS06BDPLUS_Fields]
DROP PROCEDURE [dbo].[LT_TS06BF601_Forbearance]
DROP PROCEDURE [dbo].[LT_TS06BF601_Loans]
DROP PROCEDURE [dbo].[LT_TS06BIBRXP_IDR]
DROP PROCEDURE [dbo].[LT_TS06BIBR45_IDR]
DROP PROCEDURE [dbo].[LT_TS06BICR45_IDR]
DROP PROCEDURE [dbo].[LT_TS06BICRE1_IDR]
DROP PROCEDURE [dbo].[LT_TS06BPYE45_IDR]
DROP PROCEDURE [dbo].[LT_TS06BPYEXP_IDR]
DROP PROCEDURE [dbo].[LT_TS06BRPYXP_IDR]
DROP PROCEDURE [dbo].[LT_TS06BTRT4_Name]
DROP PROCEDURE [dbo].[LT_TS06RPYE45_Deadline]
DROP PROCEDURE [dbo].[LT_TS06BRPY45_Deadline]
DROP PROCEDURE [dbo].[LT_TS11BISF_Reversed_Transaction]
DROP PROCEDURE [dbo].[LT_TS06BIDRPS_Fields]
DROP PROCEDURE [dbo].[LT_TS06BIDRF_LOANS]
DROP PROCEDURE [dbo].[LT_TS06BNEGAM_InterestAmount]
DROP PROCEDURE [dbo].[LT_TS06BRATE_Loans]
DROP PROCEDURE [dbo].[LT_TS06BTM2C_ConsolidationPayment]
DROP PROCEDURE [dbo].[LT_TS06BTM2LM]
DROP PROCEDURE [dbo].[LT_TS06BTPDSS]
DROP PROCEDURE [dbo].[LT_TS09B60P_Loans]
DROP PROCEDURE [dbo].[LT_TS09BDFDCP_Loans]
DROP PROCEDURE [dbo].[GetLetterData_TS06BD101]
DROP PROCEDURE [dbo].[GetLetterData_TS06BF101]
DROP PROCEDURE [dbo].[GetLetterData_TS06BF101C]
DROP PROCEDURE [dbo].[GetLetterData_TS06BF101J]
DROP PROCEDURE [dbo].[LT_TS06BPIFG_Loans]
DROP PROCEDURE [dbo].[LT_TS06BPIFG]


USE ECorrFed
GO

UPDATE ECorrFed..Letters SET Active = 0 WHERE Letter = 'TS06BPIFG'