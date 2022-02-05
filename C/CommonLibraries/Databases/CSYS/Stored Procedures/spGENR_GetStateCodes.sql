-- =============================================
-- Author:		Daren Beattie
-- Create date: August 23, 2011
-- Description:	Retrieves the 2-letter state codes for the 50 U.S. states, plus DC, and optionally for all U.S. territories as well.
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetStateCodes]
	-- Add the parameters for the stored procedure here
	@IncludeTerritories BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Code
	FROM GENR_LST_States
	WHERE @IncludeTerritories = 1
		OR IsDomestic = 1
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetStateCodes] TO [db_executor]
    AS [dbo];

