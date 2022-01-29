--users with access to System Access Database
SELECT DISTINCT
	SDU.LastName
	,SDU.FirstName
FROM
	[BSYS].[dbo].[SYSA_LST_DBAccess] DBA
	INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] SDU
		ON DBA.EmailAddress = SDU.WindowsUserName
	

--UHEAA employees (by default all have access to ACDC/NeedHelp)
SELECT DISTINCT
	SDU.LastName
	,SDU.FirstName
	,BU.Name AS Department
	,SLA.RoleName
FROM
	[CSYS].[dbo].[SYSA_DAT_Users] SDU
	LEFT JOIN [CSYS].[dbo].[GENR_LST_BusinessUnits] BU
		ON SDU.BusinessUnit = BU.ID
	LEFT JOIN [CSYS].[dbo].[SYSA_LST_Role] SLA
		ON SLA.RoleID = SDU.Role
WHERE
	Status= 'Active'
	and BusinessUnit not in (23,37,38)
ORDER BY
	LastName
