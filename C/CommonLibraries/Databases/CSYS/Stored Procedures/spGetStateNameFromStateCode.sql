-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/26/2012
-- Description:	Will return the full state Name for a given state abrevation
-- =============================================
CREATE PROCEDURE [dbo].[spGetStateNameFromStateCode]
	
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
    ON OBJECT::[dbo].[spGetStateNameFromStateCode] TO [db_executor]
    AS [dbo];

