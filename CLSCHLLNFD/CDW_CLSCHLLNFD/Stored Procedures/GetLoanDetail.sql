CREATE PROCEDURE [clschllnfd].[GetLoanDetail]
	@BF_SSN VARCHAR(9),
	@LN_SEQ INT
AS

BEGIN
	SELECT DISTINCT
		COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
		CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 110) AS [Disbursement Date],
		LN10.LA_CUR_PRI AS [Current Principal Balance]
	FROM 
		LN10_LON LN10
		LEFT JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
			AND FMT.FmtName = '$LNPROG'
	WHERE
		LN10.LN_SEQ = @LN_SEQ
		AND LN10.BF_SSN = @BF_SSN
END
RETURN 0