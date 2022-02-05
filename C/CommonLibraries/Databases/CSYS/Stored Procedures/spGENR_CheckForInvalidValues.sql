
CREATE PROCEDURE [dbo].[spGENR_CheckForInvalidValues]
		@ValueKey	Varchar(50),
		@Value		Varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	Count(Value) AS ValuesFound
	FROM	GENR_DAT_InvalidValues
	WHERE	ValueKey = @ValueKey
			AND Value = @Value

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_CheckForInvalidValues] TO [db_executor]
    AS [dbo];

