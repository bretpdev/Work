CREATE PROCEDURE dbo.spGENRBusinessUnitOrder

AS

select A.BusinessUnit, A.Type, A.Parent , C.FirstName + ' ' + C.LastName as Manager
from GENR_LST_BusinessUnits A
inner join GENR_REF_BU_Agent_Xref B
	on A.BusinessUnit = B.BusinessUnit
	and B.Role = 'Manager'
inner join SYSA_LST_Users C
	on B.WindowsUserID = C.WindowsUserName

where parent is not null
or A.BusinessUnit = 'Executive Director of UHEAA'