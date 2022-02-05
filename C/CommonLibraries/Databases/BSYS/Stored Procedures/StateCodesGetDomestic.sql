CREATE PROCEDURE [dbo].[StateCodesGetDomestic]
AS
	SELECT Code FROM GENR_LST_States WHERE Domestic = 'Y'
RETURN 0
