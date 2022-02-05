-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/28/2012
-- Description:	Returns a list of all the users in the SYSA_DAT_Users table
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetUserData] 
	@Status varchar(50) = ''
AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Status <> ''
		SELECT *
		FROM SYSA_DAT_Users
		WHERE Status = @Status
	ELSE
		SELECT *
		FROm SYSA_DAT_Users
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetUserData] TO [db_executor]
    AS [dbo];

