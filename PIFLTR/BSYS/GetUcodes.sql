CREATE PROCEDURE [dbo].[GetUcodes]

AS

SELECT 
	LenderID 
FROM 
	BSYS.dbo.GENR_REF_LenderAffiliation 
WHERE 
	Affiliation = 'UHEAA'
RETURN 0
