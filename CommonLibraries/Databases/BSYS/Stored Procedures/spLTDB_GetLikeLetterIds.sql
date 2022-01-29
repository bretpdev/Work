
CREATE PROCEDURE [dbo].[spLTDB_GetLikeLetterIds]
	@CostCenter			VARCHAR(50),
	@Duplex				BIT,
	@Pages				NUMERIC(18,0)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT	ID
	FROM	LTDB_DAT_CentralPrintingDocData
	WHERE	UHEAACostCenter = @CostCenter
			AND Duplex = @Duplex
			AND Pages = @Pages
			AND (Instructions IS NULL OR CAST(Instructions AS VARCHAR(50)) = '') 

END