
/********************************************************
*Routine Name	: [dbo].[spSYSA_GetKeyID]
*Purpose		: Returns the ID of a key
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/16/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_GetKeyID]
	-- Add the parameters for the stored procedure here
	  @UserKey varchar(100) = ''
	, @Application varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID FROM SYSA_LST_UserKeys
	WHERE UserKey = @UserKey
	AND Application = @Application

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetKeyID] TO [db_executor]
    AS [dbo];

