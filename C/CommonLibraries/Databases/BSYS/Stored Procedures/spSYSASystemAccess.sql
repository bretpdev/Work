CREATE PROCEDURE dbo.spSYSASystemAccess 
@WindowsID			varchar(50)

AS

SELECT A.FirstName + ' ' + A.LastName as [User], 
	C.[System] as [System] 
FROM SYSA_LST_Users A 
JOIN SYSA_LST_UserIDInfo B ON A.WindowsUserName = B.WindowsUserName 
JOIN SYSA_REF_UserID_Systems C ON B.UserID = C.UserID 
WHERE C.[System] = @WindowsID
	AND B.[Date Access Removed] IS NULL

UNION 

SELECT A.FirstName + ' ' + A.LastName as [User], 
	B.[System] as [System] 
FROM SYSA_LST_Users A
JOIN SYSA_REF_User_Systems B ON A.WindowsUserName = B.WindowsUserName 
WHERE B.[System] = @WindowsID
	AND B.DtAccessRemoved IS NULL