
CREATE PROCEDURE [dbo].[spMD_Get411]
	@AccountNumber		varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--select comment
	SELECT	BX_CMT AS Comment
	FROM	dbo.AY01_M1411
	WHERE	DF_SPE_ACC_ID = @AccountNumber

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_Get411] TO [UHEAA\Imaging Users]
    AS [dbo];

