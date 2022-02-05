-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/06/2014
-- Description:	Gets the country name based on the given code.
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetCountryNameForCode] 

@Code nchar(2)

AS
BEGIN

	SET NOCOUNT ON;

    
	SELECT 
		name
	FROM
		[dbo].[GENR_LST_Countries]
	WHERE 
		code = @code
END
