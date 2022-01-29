﻿
/********************************************************
*Routine Name	: [dbo].[spSYSA_DeleteRole]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/25/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_DeleteRole]
	-- Add the parameters for the stored procedure here
	  @RoleID int
	, @SqlUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentDate AS Datetime = GetDate()

    -- Insert statements for procedure here
	UPDATE SYSA_LST_Role
	SET RemovedBy = @SqlUserID, EndDate = @CurrentDate
	WHERE RoleID = @RoleID

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_DeleteRole] TO [db_executor]
    AS [dbo];
