CREATE PROCEDURE [dbo].[LT_TS06BF101C_Forbearances]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		THREE.FOR_TYP AS [TYPE],
		THREE.LD_FOR_BEG AS [BEGIN DATE],
		THREE.LD_FOR_END AS [END DATE],
		LN10.LN_SEQ AS [LOAN SEQ],
		FMT.Label AS [LOAN PROGRAM],
		LN10.LD_LON_1_DSB AS [FIRST DISBURSED],
		LN10.LA_CUR_PRI AS [CURRENT PRINCIPAL]
	FROM
		--ordered and numbered list of forbearances
		(
			SELECT
				FRBS.DF_SPE_ACC_ID,
				FRBS.FOR_TYP,
				FRBS.LD_FOR_BEG,
				FRBS.LD_FOR_END,
				ROW_NUMBER() OVER --group by account, sort by begin date with most recent first, and add seq no
					(
						PARTITION BY 
							FRBS.DF_SPE_ACC_ID 
						ORDER BY 
							FRBS.DF_SPE_ACC_ID, 
							CAST(FRBS.LD_FOR_BEG AS DATE) DESC
					) AS FORB_SEQ
			FROM 
				--select distinct forbearances 
				(
					SELECT DISTINCT
						FB10.DF_SPE_ACC_ID,
						FB10.FOR_TYP,
						FB10.LD_FOR_BEG,
						FB10.LD_FOR_END
					FROM
						FB10_Forbearance FB10
				) FRBS
		) THREE
		JOIN LN10_Loan LN10
			ON THREE.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID
			AND THREE.FORB_SEQ <= 3 --only select the top 3 forbearances by seq no added above
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BF101C_Forbearances])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BF101C_Forbearances] TO [db_executor]
    AS [dbo];

