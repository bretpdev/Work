USE ReportServer
GO

--Existing subscriptions to Diane or Dean directly.
--Update these to be the email of the ROLE - Document Processing Supervisor ONCE THAT ROLE EXISTS

SELECT 
	c.Path 
FROM 
	Subscriptions S 
	INNER JOIN Catalog C ON C.ItemID = S.Report_OID 
WHERE 
	S.Description LIKE '%dblair%' 
	OR S.Description LIKE '%dcox%'
ORDER BY 
	c.Path

--It appears that the root path of the Operations Folder just needs security updates to allow the new role once created

SELECT 
	C.[Path], 
	C.[Name] 
FROM 
	Users U 
	INNER JOIN PolicyUserRole PUR ON PUR.UserID = U.UserID 
	INNER JOIN Catalog C ON C.PolicyID = PUR.PolicyID 
WHERE 
	U.UserName = 'ROLE - Operations - Supervisor'