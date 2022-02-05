
CREATE PROCEDURE dbo.spDSPT_UserAndAppAccess 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.WindowsUserName, 
			A.FirstName + ' ' + A.LastName as UserName, 
			B.AppOrModName 
	FROM SYSA_LST_Users A 
	JOIN SYSA_REF_User_AppAndMod B ON A.WindowsUserName = B.WindowsUserName 
	WHERE A.WindowsUserName NOT IN (	Select WindowsUserID 
										FROM GENR_REF_BU_Agent_Xref 
										WHERE BusinessUnit = 'Former Employee'	)
END