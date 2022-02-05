-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/10/2013
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[spGetUserInfoForCentralizedPrint]
	
	@WindowsUserId As Varchar(100)
	
AS
BEGIN

	SET NOCOUNT ON;

SELECT TOP 1
	A.BusinessUnit as UsersBusinessUnit,
	A.BusinessUnit as OriginalGatheredBusinessUnit, 
    A.AssociatedEmailAddr as UsersBuSentFromEmail,
    C.FirstName as UsersFirstName 
FROM GENR_LST_BusinessUnitEmailAddrs A 
	JOIN GENR_REF_BU_Agent_Xref B ON A.BusinessUnit = B.BusinessUnit 
	JOIN SYSA_LST_Users C ON B.WindowsUserID = C.WindowsUserName
WHERE B.WindowsUserID = @WindowsUserId AND B.Role = 'Member Of'

END