﻿-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns a list of address source descriptions
-- =============================================
CREATE PROCEDURE spGENR_GetAddressSourceDescriptions 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	[description]
	FROM	GENR_LST_AddressSources
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetAddressSourceDescriptions] TO [db_executor]
    AS [dbo];
