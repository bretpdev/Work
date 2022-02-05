
CREATE PROCEDURE [dbo].[spGENR_GetDomesticIndicator]
	
	@StateCode		CHAR(2)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	COUNT(*) AS CNT 
	FROM	GENR_LST_States
	WHERE	CODE = @StateCode
			AND Domestic = 'Y'
END