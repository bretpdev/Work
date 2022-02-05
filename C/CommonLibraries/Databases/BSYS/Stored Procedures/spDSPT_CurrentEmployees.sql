
CREATE PROCEDURE dbo.spDSPT_CurrentEmployees 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT A.FirstName + ' ' + A.LastName as 'User Name', 
			A.WindowsUserName as 'Windows User ID', 
			B.Role, 
			B.BusinessUnit as 'Business Unit' 
	FROM GENR_REF_BU_Agent_Xref B 
	JOIN SYSA_LST_Users A ON B.WindowsUserID = A.WindowsUserName 
	WHERE B.BusinessUnit <> 'Former Employee' AND A.PseudoUser = 0
END