
/********************************************************
*Routine Name	: [dbo].[spGetCommonExceptionCourt]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_GetCommonExceptionCourt]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SqlUserID FROM GENR_DAT_CommonExceptionCourt

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetCommonExceptionCourt] TO [db_executor]
    AS [dbo];

