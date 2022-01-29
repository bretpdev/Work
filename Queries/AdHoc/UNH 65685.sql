SELECT 
	U.UserName, 
	R.RoleName, 
	C.[Path] AS ReportPath,
	S.[Description] AS Subscriptions
FROM 
	ReportServer..Users U 
	INNER JOIN ReportServer..PolicyUserRole PUR
		ON PUR.UserID = U.UserID
	INNER JOIN ReportServer..Roles R
		ON R.RoleID = PUR.RoleID
	INNER JOIN ReportServer..Catalog C
		ON C.PolicyID = PUR.PolicyID
	LEFT JOIN ReportServer..Subscriptions S
		ON S.Report_OID = C.ItemID
WHERE 
	U.UserName IN('UHEAA\ROLE - CS Processing Supervisor')
	AND C.PolicyRoot = 1
ORDER BY
	C.[Path]
