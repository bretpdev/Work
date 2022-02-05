

CREATE PROCEDURE [dbo].[spDSPT_OpsSuprtAssign]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT B.FirstName + ' ' + B.LastName as 'Support Person''s Name', 
			A.BusinessUnit as 'Business Unit' 
	FROM GENR_REF_BU_Agent_Xref A 
	JOIN SYSA_LST_Users B ON A.WindowsUserID = B.WindowsUserName 
	WHERE A.Role = 'OS Assigned'
	ORDER BY B.FirstName, B.LastName
END