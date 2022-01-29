﻿CREATE PROCEDURE [dbo].[LT_TS06BAPIDR_Loans]
	@AccountNumber CHAR(10),
	@IsCoborrower BIT = 0
AS 
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE	@LetterId VARCHAR(10) = 'TS06BAPIDR'
DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0 
	BEGIN
		SELECT
			COALESCE(FMT.Label, RS.IC_LON_PGM) AS [Loan Program], 
			CONVERT(VARCHAR(10), RS.LD_LON_1_DSB, 101) AS [First Disbursement Date],
			'$' + CAST(RS.LA_LON_AMT_GTR AS VARCHAR) AS [Original Balance],
			'$' + CAST(RS.LA_CUR_PRI AS VARCHAR) AS [Current Principal],
			RS.LR_ITR AS [Interest Rate],
			CASE 
				WHEN RS.LC_TYP_SCH_DIS IN ('I5', 'IA') THEN 'REPAYE'
				WHEN RS.LC_TYP_SCH_DIS IN ('CA','CP') THEN 'PAYE'
				WHEN RS.LC_TYP_SCH_DIS IN ('CQ','C1','C2','C3') THEN 'ICR'
				WHEN RS.LC_TYP_SCH_DIS IN ('IB','IL','IP','I3') THEN 'IBR'
				ELSE 'IDR'
			END AS [Schedule Type],
			'$' + CAST(RS.LA_TOT_RPD_DIS AS VARCHAR) AS [Total Repay Amount],
			'$' + CAST(COALESCE(RS.LA_ANT_CAP,0) AS VARCHAR) AS [Anticipated Cap],
			CONVERT(VARCHAR(10), RS.TermStartDate, 101) AS [Due Date],
			RS.LN_RPS_TRM AS [Repay Term in Months],
			'$' + CAST(RS.LA_RPS_ISL AS VARCHAR) AS [Installment Amount]
		FROM
			calc.RepaymentSchedules RS
			INNER JOIN PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = RS.BF_SSN
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
					(
						SELECT
							AY10.BF_SSN,
							MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
							MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM 
							AY10_BR_LON_ATY AY10
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = AY10.BF_SSN
						WHERE
							AY10.PF_REQ_ACT = @PF_REQ_ACT
							AND PD10.DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							AY10.BF_SSN
					)AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			)LN85
				ON LN85.BF_SSN = RS.BF_SSN
				AND LN85.LN_SEQ = RS.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON RS.IC_LON_PGM = FMT.Start
				AND FMT.FmtName = '$LNPROG'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
		ORDER BY
			RS.LN_SEQ,
			RS.TermStartDate
	END
ELSE
	BEGIN
		SELECT
			COALESCE(FMT.Label, RS.IC_LON_PGM) AS [Loan Program], 
			CONVERT(VARCHAR(10), RS.LD_LON_1_DSB, 101) AS [First Disbursement Date],
			'$' + CAST(RS.LA_LON_AMT_GTR AS VARCHAR) AS [Original Balance],
			'$' + CAST(RS.LA_CUR_PRI AS VARCHAR) AS [Current Principal],
			RS.LR_ITR AS [Interest Rate],
			CASE 
				WHEN RS.LC_TYP_SCH_DIS IN ('I5', 'IA') THEN 'REPAYE'
				WHEN RS.LC_TYP_SCH_DIS IN ('CA','CP') THEN 'PAYE'
				WHEN RS.LC_TYP_SCH_DIS IN ('CQ','C1','C2','C3') THEN 'ICR'
				WHEN RS.LC_TYP_SCH_DIS IN ('IB','IL','IP','I3') THEN 'IBR'
				ELSE 'IDR'
			END AS [Schedule Type],
			'$' + CAST(RS.LA_TOT_RPD_DIS AS VARCHAR) AS [Total Repay Amount],
			'$' + CAST(COALESCE(RS.LA_ANT_CAP,0) AS VARCHAR) AS [Anticipated Cap],
			CONVERT(VARCHAR(10), RS.TermStartDate, 101) AS [Due Date],
			RS.LN_RPS_TRM AS [Repay Term in Months],
			'$' + CAST(RS.LA_RPS_ISL AS VARCHAR) AS [Installment Amount]
		FROM
			LN20_EDS LN20
			INNER JOIN PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN20.LF_EDS
			INNER JOIN calc.RepaymentSchedules RS
				ON RS.BF_SSN = LN20.BF_SSN
				AND RS.LN_SEQ = LN20.LN_SEQ
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
					(
						SELECT
							AY10.BF_SSN,
							MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
							MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM 
							AY10_BR_LON_ATY AY10
							INNER JOIN LN20_EDS LN20
								ON LN20.BF_SSN = AY10.BF_SSN
								AND LN20.LC_EDS_TYP = 'M'
								AND LN20.LC_STA_LON20 = 'A'
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = LN20.LF_EDS
						WHERE
							AY10.PF_REQ_ACT = @PF_REQ_ACT
							AND PD10.DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							AY10.BF_SSN
					)AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			)LN85
				ON LN85.BF_SSN = RS.BF_SSN
				AND LN85.LN_SEQ = RS.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON RS.IC_LON_PGM = FMT.Start
				AND FMT.FmtName = '$LNPROG'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		ORDER BY
			RS.LN_SEQ,
			RS.TermStartDate
	END
END
GO

