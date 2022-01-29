-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/26/2012
-- Description:	Will return the full state Name for a given state abrevation
-- =============================================
create PROCEDURE [dbo].[spGENR_GetStateNameForStateCode]
	
	@StateCode as Varchar(2)

AS
BEGIN

	SET NOCOUNT ON;

	Select [Description]
	From dbo.GENR_LST_States
	Where Code = @StateCode
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetStateNameForStateCode] TO [db_executor]
    AS [dbo];

