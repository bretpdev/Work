-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns a list of country names
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetCountryNames]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	name
	FROM	GENR_LST_Countries
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetCountryNames] TO [db_executor]
    AS [dbo];

