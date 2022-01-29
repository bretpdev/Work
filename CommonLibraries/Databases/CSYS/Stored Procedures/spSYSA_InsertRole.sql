
/********************************************************
*Routine Name	: [dbo].[spSYSA_InsertRole]
*Purpose		:  
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/25/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_InsertRole]
	-- Add the parameters for the stored procedure here
	  @RoleName nvarchar(64)
	, @SqlUserID int  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentDate AS DateTime = GetDate()

    -- Insert statements for procedure here
	INSERT INTO SYSA_LST_Role(RoleName, AddedBy, StartDate)
	VALUES(@RoleName, @SqlUserID, @CurrentDate)
	
	SELECT @@IDENTITY

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_InsertRole] TO [db_executor]
    AS [dbo];

