
-- =============================================
-- Author:		Daren Beattie
-- Create date: December 12, 2011
-- Description:	Retrieves the SqlUserId from the SYSA_DAT_Users table based on Windows User Name and status (only the active record is retrieved).
-- =============================================
CREATE PROCEDURE [dbo].[spACDC_GetSquid]
	-- Add the parameters for the stored procedure here
	@WindowsUserName VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SqlUserId
	FROM SYSA_DAT_Users
	WHERE WindowsUserName = @WindowsUserName
	AND [Status] = 'Active'
END