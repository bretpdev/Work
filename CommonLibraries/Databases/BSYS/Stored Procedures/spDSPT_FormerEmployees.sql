
CREATE PROCEDURE dbo.spDSPT_FormerEmployees 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT A.FirstName + ' ' + A.LastName as 'User Name', 
			A.WindowsUserName as 'Windows User ID' 
	FROM SYSA_LST_Users A 
	JOIN GENR_REF_BU_Agent_Xref B ON A.WindowsUserName = B.WindowsUserID 
	WHERE BusinessUnit = 'Former Employee'
END