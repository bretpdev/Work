-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/30/2014
-- Description:	Will get all foreign codes from [dbo].[ForeignCodesAndCountries]
-- =============================================
CREATE PROCEDURE [dbo].[GetForeignCodes]

AS
BEGIN

	SET NOCOUNT ON;

    
	SELECT 
		code
	FROM
		[GENR_LST_Countries]
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetForeignCodes] TO [db_executor]
    AS [dbo];

