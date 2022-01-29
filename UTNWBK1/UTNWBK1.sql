USE CDW
GO

BEGIN TRY
BEGIN TRANSACTION

	DECLARE @Today DATE = GETDATE();
	DECLARE @ARC VARCHAR(5) = 'BKNFB';
	DECLARE @SCRIPTID VARCHAR(9) = 'UTNWBK1';
	DECLARE @R2Comment VARCHAR(200) = 'Bankruptcy Forbearance but not in bankruptcy status.';
	DECLARE @R3Comment VARCHAR(200) = 'Bankruptcy status but not in bankruptcy forbearance.';
	DECLARE @ArcAddProcessingId INT;
	DECLARE @ArcLoan TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10));
	DECLARE @R2 TABLE(DF_SPE_ACC_ID VARCHAR(10), BF_SSN VARCHAR(9), LN_SEQ SMALLINT);
	DECLARE @R3 TABLE(DF_SPE_ACC_ID VARCHAR(10), BF_SSN VARCHAR(9), LN_SEQ SMALLINT);

	INSERT INTO @R2
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		LN10.BF_SSN,
		LN10.LN_SEQ
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
		INNER JOIN
		(
			SELECT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				CAST(LN60.LD_FOR_APL AS DATE) AS LD_FOR_APL
			FROM
				CDW..FB10_BR_FOR_REQ FB10
				INNER JOIN CDW..LN60_BR_FOR_APV LN60
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					AND LN60.LC_FOR_RSP != '003'
					AND LN60.LC_STA_LON60 = 'A'
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '10'
				AND CAST(LN60.LD_FOR_END AS DATE) >= @Today
		) Forb
			ON Forb.BF_SSN = LN10.BF_SSN
			AND Forb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN CDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA IN ('20','21')
		LEFT JOIN
		(
			SELECT 
				LN85.BF_SSN,
				LN85.LN_SEQ,
				CAST(AY10.LD_ATY_REQ_RCV AS DATE) AS LD_ATY_REQ_RCV
			FROM
				CDW..AY10_BR_LON_ATY AY10
				INNER JOIN CDW..LN85_LON_ATY LN85
					ON LN85.BF_SSN = AY10.BF_SSN
					AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = 'BKNFB'
				AND AY10.LC_STA_ACTY10 = 'A'
		) AY10
			ON AY10.BF_SSN = LN10.BF_SSN
			AND AY10.LN_SEQ = LN10.LN_SEQ
			AND AY10.LD_ATY_REQ_RCV >= Forb.LD_FOR_APL
	WHERE
		DW01.BF_SSN IS NULL
		AND AY10.BF_SSN IS NULL;

	/* R3 */
	INSERT INTO @R3
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		LN10.BF_SSN,
		LN10.LN_SEQ
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND CAST(ISNULL(LN10.LD_END_GRC_PRD,'1900-01-01') AS DATE) <= @Today --Exclude grace end future
		INNER JOIN CDW..PD24_PRS_BKR PD24
			ON PD24.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD24.DC_BKR_STA IN ('04','06')
		INNER JOIN CDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA IN ('20','21')
		LEFT JOIN
		(
			SELECT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				CAST(LN60.LD_FOR_APL AS DATE) AS LD_FOR_APL
			FROM
				CDW..FB10_BR_FOR_REQ FB10
				INNER JOIN CDW..LN60_BR_FOR_APV LN60
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					AND LN60.LC_FOR_RSP != '003'
					AND LN60.LC_STA_LON60 = 'A'
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND CAST(LN60.LD_FOR_BEG AS DATE) <= @Today
				AND CAST(LN60.LD_FOR_END AS DATE) >= @Today	
		) Forb
			ON Forb.BF_SSN = LN10.BF_SSN
			AND Forb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				CAST(LN50.LD_DFR_APL AS DATE) AS LD_DFR_APL
			FROM
				CDW..DF10_BR_DFR_REQ DF10
				INNER JOIN CDW..LN50_BR_DFR_APV LN50
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					AND LN50.LC_DFR_RSP != '003'
					AND LN50.LC_STA_LON50 = 'A'
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND CAST(LN50.LD_DFR_BEG AS DATE) <= @Today
				AND CAST(LN50.LD_DFR_END AS DATE) >= @Today
		) Defer
			ON Defer.BF_SSN = LN10.BF_SSN
			AND Defer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				LN85.BF_SSN,
				LN85.LN_SEQ,
				CAST(AY10.LD_ATY_REQ_RCV AS DATE) AS LD_ATY_REQ_RCV
			FROM
				CDW..AY10_BR_LON_ATY AY10
				INNER JOIN CDW..LN85_LON_ATY LN85
					ON LN85.BF_SSN = AY10.BF_SSN
					AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = 'BKNFB'
				AND AY10.LC_STA_ACTY10 = 'A'
		) AY10
			ON AY10.BF_SSN = LN10.BF_SSN
			AND AY10.LN_SEQ = LN10.LN_SEQ
			AND AY10.LD_ATY_REQ_RCV >= CAST(PD24.DD_BKR_STA AS DATE)
	WHERE
		Forb.BF_SSN IS NULL
		AND Defer.BF_SSN IS NULL
		AND AY10.BF_SSN IS NULL;

	/* Add record into AAP */
	INSERT INTO CLS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
	OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcLoan(ArcAddProcessingId, AccountNumber)
	SELECT DISTINCT
		0, --ByLoan
		R2.DF_SPE_ACC_ID,
		@Arc,
		@ScriptId,
		GETDATE(),
		@R2Comment,
		0,
		0,
		GETDATE(),
		SUSER_NAME()
	FROM
		@R2 R2
		LEFT JOIN CLS..ArcAddProcessing AAP
			ON AAP.AccountNumber = R2.DF_SPE_ACC_ID
			AND AAP.ARC = @Arc
			AND AAP.ScriptId = @ScriptId
			AND AAP.Comment = @R2Comment
			AND CAST(AAP.CreatedAt AS DATE) = @Today
	WHERE
		AAP.AccountNumber IS NULL

	/* Add record into AAP loan selection */
	INSERT INTO CLS..ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
	SELECT
		AL.ArcAddProcessingId,
		R2.LN_SEQ
	FROM
		@R2 R2
		INNER JOIN @ArcLoan AL
			ON AL.AccountNumber = R2.DF_SPE_ACC_ID;

	DELETE FROM @ArcLoan; --Clear out ArcLoan to use it again

	INSERT INTO CLS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
	OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcLoan(ArcAddProcessingId, AccountNumber)
	SELECT DISTINCT
		0, --ByLoan
		R3.DF_SPE_ACC_ID,
		@Arc,
		@ScriptId,
		GETDATE(),
		@R3Comment,
		0,
		0,
		GETDATE(),
		SUSER_NAME()
	FROM
		@R3 R3
		LEFT JOIN CLS..ArcAddProcessing AAP
			ON AAP.AccountNumber = R3.DF_SPE_ACC_ID
			AND AAP.ARC = @Arc
			AND AAP.ScriptId = @ScriptId
			AND AAP.Comment = @R3Comment
			AND CAST(AAP.CreatedAt AS DATE) = @Today
	WHERE
		AAP.AccountNumber IS NULL;

	/* Add record into AAP loan selection */
	INSERT INTO CLS..ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
	SELECT
		AL.ArcAddProcessingId,
		R3.LN_SEQ
	FROM
		@R3 R3
		INNER JOIN @ArcLoan AL
			ON AL.AccountNumber = R3.DF_SPE_ACC_ID;

	COMMIT TRANSACTION;

END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = (SELECT ERROR_MESSAGE());
	PRINT 'UTNWBK1.sql encountered an error.  Transaction not committed. Error: ' + @EM;
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;