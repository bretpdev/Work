
CREATE PROCEDURE dbo.spDSPT_LenderUserIDs
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.UserID, 
		B.FirstName + ' ' + B.LastName as 'User Name', 
		C.InstitutionName, 
		A.[Date Access Removed] 
	FROM SYSA_LST_UserIDInfo A 
	LEFT JOIN SYSA_LST_Users B ON A.WindowsUserName = B.WindowsUserName 
	LEFT JOIN SYSA_LST_Institutions C ON A.InstitutionID = C.InstitutionID 
	WHERE A.UserID LIKE 'BK%' 
	ORDER BY C.InstitutionName
END