-- =============================================
-- Author:		Jay Davis
-- Create date: 02/25/2013
-- Description:	Returns a list of suffixes
-- =============================================
create PROCEDURE [dbo].[spGENR_GetSuffixes] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	suffix
	FROM	GENR_LST_Suffixes
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetSuffixes] TO [db_executor]
    AS [dbo];

