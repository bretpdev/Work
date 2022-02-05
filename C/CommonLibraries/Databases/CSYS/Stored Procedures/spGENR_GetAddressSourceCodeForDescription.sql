-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns the address source code for the description provided
-- =============================================
CREATE PROCEDURE spGENR_GetAddressSourceCodeForDescription
	-- Add the parameters for the stored procedure here
	@Description varchar(35)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	code
	FROM	dbo.GENR_LST_AddressSources
	WHERE	[description] = @Description
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetAddressSourceCodeForDescription] TO [db_executor]
    AS [dbo];

