CREATE PROCEDURE [dbo].[spCentralizedPrintingGetManagerEmail]
	@BusinessUnit varchar(50)
AS
	SELECT WindowsUserID + '@utahsbr.edu' as ManagersEmail From dbo.GENR_REF_BU_Agent_Xref WHERE ROLE = 'Manager' and BusinessUnit = @BusinessUnit  
RETURN 0
