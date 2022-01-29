
CREATE PROCEDURE [projectrequest].[GetDepartments]
AS

SELECT	
	BU.BusinessUnit
FROM	
	[BSYS].[dbo].[GENR_LST_BusinessUnits] BU
WHERE 
	BU.[PseudoBU] = 'N' 
	AND BU.[type] = 'Group' 
	AND BU.[BusinessUnit] != 'UESP Accounting'