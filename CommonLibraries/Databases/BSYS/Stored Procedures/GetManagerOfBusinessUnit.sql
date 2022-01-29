CREATE PROCEDURE [dbo].[GetManagerOfBusinessUnit]
	@BusinessUnit nvarchar(50)
AS
	select top 1 utid.UserID
	  from BSYS.dbo.GENR_REF_BU_Agent_Xref agent
	  join BSYS.dbo.SYSA_LST_UserIDInfo utid on utid.WindowsUserName = agent.WindowsUserID
	 where BusinessUnit = @BusinessUnit
	   and [Role] = 'Manager'
	   and utid.[Date Access Removed] is null

RETURN 0