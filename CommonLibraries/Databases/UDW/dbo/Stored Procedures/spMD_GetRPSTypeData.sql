CREATE PROCEDURE [dbo].[spMD_GetRPSTypeData]
	@AccountNumber					VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SELECT DISTINCT 
		COALESCE(FMT.label, LN65.LC_TYP_SCH_DIS) AS TYP_SCH_DIS
	FROM 
		UDW.dbo.PD10_PRS_NME PD10
		INNER JOIN UDW..LN65_LON_RPS LN65
			ON LN65.BF_SSN = PD10.DF_PRS_ID
			AND LN65.LC_STA_LON65 = 'A'
		LEFT JOIN UDW..FormatTranslation FMT
			ON FMT.Start = LN65.LC_TYP_SCH_DIS
			AND FMT.FmtName = '$SCHTYP'
	WHERE 
		PD10.DF_SPE_ACC_ID = @AccountNumber 
		AND COALESCE(FMT.label, LN65.LC_TYP_SCH_DIS, '') != ''
	/*Might need to consider looking at current gradation for this if this doesnt seem correct*/

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetRPSTypeData] TO [UHEAA\Imaging Users]
    AS [dbo];

