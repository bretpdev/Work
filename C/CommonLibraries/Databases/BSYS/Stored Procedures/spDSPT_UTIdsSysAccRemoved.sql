
CREATE PROCEDURE dbo.spDSPT_UTIdsSysAccRemoved 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT A.FirstName + ' ' + A.LastName as 'User Name', 
			B.UserID, B.[Date Access Removed] 
	FROM SYSA_LST_Users A 
	JOIN dbo.SYSA_LST_UserIDInfo B ON A.WindowsUserName = B.WindowsUserName 
	WHERE B.[Date Access Removed] IS NOT NULL
END