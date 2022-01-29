CREATE PROCEDURE [dbo].[LT_US06BTRT4_Loans]
	@LetterId VARCHAR(10),
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT 
			ISNULL(FMT.Label, RS.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR, RS.LD_LON_1_DSB, 101) AS [First Disbursement Date],
			RS.LA_LON_AMT_GTR AS [Original Balance],
			RS.LA_CUR_PRI AS [Current Principal],
			RS.LR_ITR AS [Interest Rate],
			RS.LC_TYP_SCH_DIS AS [Schedule Type],
			RS.LA_TOT_RPD_DIS AS [Total Repay Amount],
			ISNULL(RS.LA_ANT_CAP, '0.00') AS [Anticipated Cap],
			CONVERT(VARCHAR, RS.TermStartDate, 101) AS [Due Date],
			RS.LN_RPS_TRM AS [Repay Term in Months],
			RS.LA_RPS_ISL AS [Installment Amount]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN calc.RepaymentSchedules RS
				ON RS.BF_SSN = PD10.DF_PRS_ID
				AND RS.CurrentGradation = 1
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND AY10.BF_SSN = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = RS.BF_SSN
				AND LN85.LN_SEQ = RS.LN_SEQ 
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = RS.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
			LEFT JOIN FormatTranslation FMTRepay
				ON FMTRepay.Start = RS.LC_TYP_SCH_DIS
				AND FMTRepay.FmtName = '$SCHTYP'
		WHERE
			PD10.DF_PRS_ID = @BF_SSN
		ORDER BY
			RS.LN_SEQ,
			RS.TermStartDate
	END
ELSE
	BEGIN
		SELECT 
			ISNULL(FMT.Label, RS.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR, RS.LD_LON_1_DSB, 101) AS [First Disbursement Date],
			RS.LA_LON_AMT_GTR AS [Original Balance],
			RS.LA_CUR_PRI AS [Current Principal],
			RS.LR_ITR AS [Interest Rate],
			RS.LC_TYP_SCH_DIS AS [Schedule Type],
			RS.LA_TOT_RPD_DIS AS [Total Repay Amount],
			ISNULL(RS.LA_ANT_CAP, '0.00') AS [Anticipated Cap],
			CONVERT(VARCHAR, RS.TermStartDate, 101) AS [Due Date],
			RS.LN_RPS_TRM AS [Repay Term in Months],
			RS.LA_RPS_ISL AS [Installment Amount]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.LC_STA_LON20 = 'A'
			INNER JOIN calc.RepaymentSchedules RS
				ON RS.BF_SSN = LN20.BF_SSN
				AND RS.LN_SEQ = LN20.LN_SEQ
				AND RS.CurrentGradation = 1
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN85.BF_SSN
						AND LN20.LN_SEQ = LN85.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
						AND LN20.LC_EDS_TYP = 'M'
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND LN20.LF_EDS = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = RS.BF_SSN
				AND LN85.LN_SEQ = RS.LN_SEQ 
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = RS.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
			LEFT JOIN FormatTranslation FMTRepay
				ON FMTRepay.Start = RS.LC_TYP_SCH_DIS
				AND FMTRepay.FmtName = '$SCHTYP'
		WHERE
			LN20.LF_EDS = @BF_SSN
		ORDER BY
			RS.LN_SEQ,
			RS.TermStartDate
	END
END
