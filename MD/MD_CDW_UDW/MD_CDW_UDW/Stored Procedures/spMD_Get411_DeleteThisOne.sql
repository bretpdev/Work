CREATE PROCEDURE [dbo].[spMD_Get411_DeleteThisOne]
	@AccountNumber		varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--select comment
	SELECT	A.LX_ATY AS Comment
	FROM	dbo.AY10_M1411 A
	WHERE	A.DF_SPE_ACC_ID = @AccountNumber
			AND A.LN_ATY_SEQ = (SELECT	MAX(LN_ATY_SEQ) 
								FROM	dbo.AY10_M1411 
								WHERE	DF_SPE_ACC_ID = @AccountNumber
								GROUP BY DF_SPE_ACC_ID)

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_Get411_DeleteThisOne] TO [Imaging Users]
    AS [dbo];

