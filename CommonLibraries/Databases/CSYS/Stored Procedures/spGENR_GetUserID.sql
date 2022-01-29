﻿
/********************************************************
*Routine Name	: [dbo].[spGENR_GetUserID]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/26/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_GetUserID]
	-- Add the parameters for the stored procedure here
	  @WindowsUserName Varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SqlUserId, BusinessUnit FROM SYSA_DAT_Users
	WHERE WindowsUserName = @WindowsUserName

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetUserID] TO [db_executor]
    AS [dbo];

