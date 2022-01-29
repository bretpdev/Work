CREATE PROCEDURE [clmpmtpst].[AddClaimPayments]
AS
	
	
	DECLARE @ClaimPayments TABLE (BF_SSN CHAR(9), LN_SEQ INT, AF_APL_ID VARCHAR(17), AF_APL_ID_SFX VARCHAR(2), PaymentAmount NUMERIC(9,2), LD_LDR_POF DATE, LC_STA_LON10 CHAR(1), LA_CUR_PRI NUMERIC(8,2), LA_ACR_NOT_PD_INT NUMERIC(9,2), LA_CLM_UNS NUMERIC(9,2), LA_CLM_INT NUMERIC(9,2), LA_CLM_PRI NUMERIC(9,2), LR_CUR_INT NUMERIC(6,3), DM_PRS_LST VARCHAR(23), LA_SPI_PD NUMERIC(9,2));
	DECLARE @Today DATE = GETDATE(),
		@TodayFormattedDc14 VARCHAR(8) = FORMAT(GETDATE(), 'ddMMyyyy'),
		@TodayFormattedOutput VARCHAR(8) = FORMAT(GETDATE(), 'MMddyyyy');
	
	INSERT INTO @ClaimPayments (BF_SSN, LN_SEQ, AF_APL_ID, AF_APL_ID_SFX, PaymentAmount, LD_LDR_POF, LC_STA_LON10, LA_CUR_PRI, LA_ACR_NOT_PD_INT, LA_CLM_UNS, LA_CLM_INT, LA_CLM_PRI, LR_CUR_INT, DM_PRS_LST, LA_SPI_PD)
	SELECT 
		DC01.BF_SSN,
		LN10.LN_SEQ,
		DC01.AF_APL_ID,
		DC01.AF_APL_ID_SFX,
		NULL, --Uncalculated PaymentAmount
		DC01.LD_LDR_POF,
		LN10.LC_STA_LON10,
		LN10.LA_CUR_PRI,
		DC01.LA_ACR_NOT_PD_INT,
		DC01.LA_CLM_UNS,
		DC01.LA_CLM_INT,
		DC01.LA_CLM_PRI,
		DC02.LR_CUR_INT,
		PD10.DM_PRS_LST,
		DC14.LA_SPI_PD
	FROM 
		ODW..DC01_LON_CLM_INF DC01
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = DC01.BF_SSN
			AND LN10.LF_LON_ALT = DC01.AF_APL_ID
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN ODW..GA10_LON_APP GA10
			ON GA10.AF_APL_ID = DC01.AF_APL_ID
			AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		LEFT JOIN ODW..DC02_BAL_INT DC02
			ON DC01.AF_APL_ID = DC02.AF_APL_ID
			AND DC01.AF_APL_ID_SFX = DC02.AF_APL_ID_SFX
			AND DC01.LF_CRT_DTS_DC10 = DC02.LF_CRT_DTS_DC10
			AND DC02.AF_APL_ID_SFX = LN10.LN_LON_ALT_SEQ
		LEFT JOIN ODW..DC14_SUP_INT_CLM DC14
			ON DC01.AF_APL_ID = DC14.AF_APL_ID
			AND DC01.AF_APL_ID_SFX = DC14.AF_APL_ID_SFX
			AND DC01.LF_CRT_DTS_DC10 = DC14.LF_CRT_DTS_DC10
	WHERE 
		GA10.AF_CUR_LON_SER_AGY = '700126'
		AND 
		(
			DC01.LD_LDR_POF = @Today
			OR DC14.LF_SPI_VT_RFR = @TodayFormattedDc14
		)
		

	/* Update records where the amount of unpaid accrued interest minus the uninsurable amount of the claim equals zero.
	For these records, use the claim principal and interest, subtracting any unpaid accrued interest, as the payment amount. */
	UPDATE
		CP
	SET
		CP.PaymentAmount = CALC.PaymentAmount
	FROM
		@ClaimPayments CP
		INNER JOIN
		(
			SELECT
				BF_SSN,
				LN_SEQ,
				SUM(ISNULL(LA_CLM_PRI, 0.00)) + SUM(ISNULL(LA_CLM_INT, 0.00)) - SUM(ISNULL(LA_ACR_NOT_PD_INT, 0.00)) AS PaymentAmount
			FROM
				@ClaimPayments CP
			GROUP BY
				BF_SSN,
				LN_SEQ
		) CALC
			ON CALC.BF_SSN = CP.BF_SSN
			AND CALC.LN_SEQ = CP.LN_SEQ
		INNER JOIN
		(
			SELECT DISTINCT
				BF_SSN
			FROM
				@ClaimPayments CP
			GROUP BY
				CP.BF_SSN
			HAVING 
				(SUM(LA_ACR_NOT_PD_INT) - SUM(LA_CLM_UNS)) = 0
		) ONELINE
			ON ONELINE.BF_SSN = CALC.BF_SSN

	/* Sum up the distinct payments for each borrower, then set all their records to that amount (that's how it will be entered into the session */
	UPDATE 
		CP
	SET
		CP.PaymentAmount = SPMTS.PaymentAmount
	FROM
		@ClaimPayments CP
		INNER JOIN
		(
			SELECT
				BF_SSN,
				SUM(PaymentAmount) AS PaymentAmount
			FROM
			(
				SELECT DISTINCT
					BF_SSN,
					PaymentAmount,
					AF_APL_ID
				FROM
					@ClaimPayments
			) DPMTS --Distinct payments
			GROUP BY
				BF_SSN
		) SPMTS
			ON SPMTS.BF_SSN = CP.BF_SSN

	/* Update all the multiline records with a calculated payment amount */
	UPDATE
		CP
	SET
		CP.PaymentAmount = MULTILINE.LA_SPI_PD
	FROM
		@ClaimPayments CP
		INNER JOIN 
		(
			SELECT
				BF_SSN,
				SUM(LA_SPI_PD) AS LA_SPI_PD
			FROM
				@ClaimPayments
			WHERE
				PaymentAmount IS NULL
			GROUP BY
				BF_SSN
		) MULTILINE
			ON MULTILINE.BF_SSN = CP.BF_SSN
	WHERE
		CP.PaymentAmount IS NULL

	INSERT INTO clmpmtpst.ClaimPayments (AccountNumber, PaymentAmount, EffectiveDate, LoanSequence, LastName)
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		CP.PaymentAmount,
		@TodayFormattedOutput AS EffectiveDate,
		CP.LN_SEQ AS LoanSequence,
		CP.DM_PRS_LST AS LastName
	FROM
		@ClaimPayments CP
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = CP.BF_SSN

RETURN 0
