-- =============================================
-- Author:		Daren Beattie
-- Create date: September 8, 2011
-- Description:	Retrieves the phone extension for the given Windows user ID
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetPhoneExtension]
	-- Add the parameters for the stored procedure here
	@WindowsUserId VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Extension
	FROM SYSA_DAT_Users
	WHERE WindowsUserName = @WindowsUserId
	AND Status = 'Active'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetPhoneExtension] TO [db_executor]
    AS [dbo];

