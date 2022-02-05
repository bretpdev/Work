-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns the code of the country for the name provided
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetCountryCodeForName]
	@Name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	code
	FROM	GENR_LST_Countries
	WHERE	name = @Name
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetCountryCodeForName] TO [db_executor]
    AS [dbo];

