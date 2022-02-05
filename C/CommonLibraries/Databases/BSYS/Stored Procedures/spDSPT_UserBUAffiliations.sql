

CREATE PROCEDURE [dbo].[spDSPT_UserBUAffiliations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT B.Firstname + ' ' + B.lastname as UserName, 
			A.Role, 
			A.BusinessUnit 
	FROM dbo.GENR_REF_BU_Agent_Xref A 
	JOIN dbo.SYSA_LST_Users B ON A.WindowsUserID = B.WindowsUserName 
		AND B.PseudoUser = 0 
	JOIN dbo.GENR_REF_BU_Agent_Xref C ON A.WindowsUserID = C.WindowsUserID 
	WHERE C.BusinessUnit <> 'Former Employee' 
	ORDER BY UserName, A.BusinessUnit
END