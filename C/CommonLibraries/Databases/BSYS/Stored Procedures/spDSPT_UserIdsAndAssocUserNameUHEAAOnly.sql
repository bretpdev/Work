
CREATE PROCEDURE dbo.spDSPT_UserIdsAndAssocUserNameUHEAAOnly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.UserID, 
			B.FirstName + ' ' + B.LastName AS UserName 
	FROM SYSA_LST_UserIDInfo A 
	JOIN SYSA_LST_Users B ON A.WindowsUserName = B.WindowsUserName 
	WHERE SUBSTRING(A.UserID,1,2) = 'UT' 
	ORDER BY A.UserID
END