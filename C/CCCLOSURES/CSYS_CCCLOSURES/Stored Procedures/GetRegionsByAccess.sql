CREATE PROCEDURE [ccclosures].[GetRegionsByAccess]
	@WindowsUserId varchar(50)
AS
	
SELECT
	RG.RegionsId,
	RG.RegionName
FROM
	ccclosures.Regions RG
	INNER JOIN ccclosures.RegionAccess RA
		ON RG.RegionsId = RA.RegionsId
	INNER JOIN ccclosures.IvrRoles IR
		ON RA.RoleId = IR.IvrRolesId --Joins to the table identity column
	INNER JOIN dbo.SYSA_LST_Role R
		ON IR.RoleId = R.RoleID --Joins to the actual role id from roles
	INNER JOIN dbo.SYSA_DAT_Users U
		ON R.RoleID = U.[Role]
WHERE
	U.WindowsUserName = @WindowsUserId

RETURN 0