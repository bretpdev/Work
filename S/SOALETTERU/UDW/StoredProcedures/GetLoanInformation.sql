CREATE PROCEDURE [soaletteru].[GetLoanInformation]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT = 0,
	@BorrowerSSN VARCHAR(9)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @IsCoborrower = 0
	BEGIN
		SELECT
			LN10.LN_SEQ AS [Loan Seq],
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [1st Disb Date],
			'$' + CONVERT(VARCHAR(20),LN10.LA_LON_AMT_GTR, 1) AS [Principal Balance at Transfer],
			'$' + CONVERT(VARCHAR(20),LN10.LA_CUR_PRI, 1) as [Current Principal Balance]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN10.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
ELSE
	BEGIN
		SELECT
			LN10.LN_SEQ AS [Loan Seq],
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [1st Disb Date],
			'$' + CONVERT(VARCHAR(20),LN10.LA_LON_AMT_GTR, 1) AS [Principal Balance at Transfer],
			'$' + CONVERT(VARCHAR(20),LN10.LA_CUR_PRI, 1) as [Current Principal Balance]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.BF_SSN = @BorrowerSSN
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN10.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END