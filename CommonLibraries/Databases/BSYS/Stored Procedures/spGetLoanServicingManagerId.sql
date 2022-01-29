

/********************************************************

*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		09/17/2012	Jarom Ryan		Will get the User Id of the Loan Servicing Manager
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGetLoanServicingManagerId]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		
 SELECT TOP 1 A.UserID
 FROM SYSA_LST_UserIDInfo A
 INNER JOIN GENR_REF_BU_Agent_Xref B ON A.WindowsUserName = B.WindowsUserID
 WHERE B.BusinessUnit = 'Account Services' AND B.Role = 'Manager'
 ORDER BY A.DateEstablished
		

	SET NOCOUNT OFF;
END