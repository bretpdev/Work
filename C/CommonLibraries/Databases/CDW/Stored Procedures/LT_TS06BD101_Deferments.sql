CREATE PROCEDURE [dbo].[LT_TS06BD101_Deferments]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		THREE.DFR_TYP AS [TYPE],
		THREE.LD_DFR_BEG AS [BEGIN DATE],
		THREE.LD_DFR_END AS [END DATE],
		LN10.LN_SEQ AS [LOAN SEQ],
		FMT.Label AS [LOAN PROGRAM],
		LN10.LD_LON_1_DSB AS [FIRST DISBURSED],
		LN10.LA_CUR_PRI AS [CURRENT PRINCIPAL]
	FROM
		--ordered and numbered list of deferments
		(
			SELECT
				DFRS.DF_SPE_ACC_ID,
				DFRS.DFR_TYP,
				DFRS.LD_DFR_BEG,
				DFRS.LD_DFR_END,
				ROW_NUMBER() OVER --group by account, sort by begin date with most recent first, and add seq no
					(
						PARTITION BY 
							DFRS.DF_SPE_ACC_ID 
						ORDER BY 
							DFRS.DF_SPE_ACC_ID, 
							CAST(DFRS.LD_DFR_BEG AS DATE) DESC
					) AS DFRB_SEQ
			FROM 
				--select distinct deferments 
				(
					SELECT DISTINCT
						DF10.DF_SPE_ACC_ID,
						DF10.DFR_TYP,
						DF10.LD_DFR_BEG,
						DF10.LD_DFR_END
					FROM
						DF10_Deferment DF10
				) DFRS
		) THREE
		JOIN LN10_Loan LN10
			ON THREE.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID
			AND THREE.DFRB_SEQ <= 3 --only select the top 3 deferments
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BD101_Deferments])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BD101_Deferments] TO [db_executor]
    AS [dbo];
