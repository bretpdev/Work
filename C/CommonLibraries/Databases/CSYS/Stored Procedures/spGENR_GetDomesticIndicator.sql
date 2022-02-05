
CREATE PROCEDURE [dbo].[spGENR_GetDomesticIndicator]
	@StateCode		CHAR(2)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--SELECT	COUNT(*) AS CNT 
	--FROM	dbo.GENR_LST_States
	--WHERE	CODE = @StateCode
	--		AND IsDomestic = 1
			
	SELECT	IsDomestic
	FROM	dbo.GENR_LST_States
	WHERE	CODE = @StateCode
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetDomesticIndicator] TO [db_executor]
    AS [dbo];

