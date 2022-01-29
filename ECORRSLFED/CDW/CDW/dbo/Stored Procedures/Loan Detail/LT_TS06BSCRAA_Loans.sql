﻿CREATE PROCEDURE [dbo].[LT_TS06BSCRAA_Loans]
	@AccountNumber CHAR(10),
	@IsCoborrower BIT = 0
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE	@LetterId VARCHAR(10) = 'TS06BSCRAA'
DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [First Disbursement Date],
			'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal Balance],
			CONVERT(VARCHAR(10),LN72.LD_ITR_EFF_BEG,101) AS [SCRA Rate Begin Date],
			CONVERT(VARCHAR(10),LN72.LD_ITR_EFF_END,101) AS [SCRA Rate End Date]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN
			(	--Adding Active Interest Rate
				SELECT
					LN72.BF_SSN, 
					LN72.LN_SEQ,
					LN72.LD_ITR_EFF_BEG,
					LN72.LD_ITR_EFF_END,
					ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ, LN72.LD_ITR_EFF_BEG, LN72.LD_ITR_EFF_END ORDER BY LD_STA_LON72 DESC) AS SEQ
				FROM
					LN72_INT_RTE_HST LN72
					INNER JOIN PD10_PRS_NME PD10 
						ON PD10.DF_PRS_ID = LN72.BF_SSN
				WHERE
					LN72.LC_STA_LON72 = 'A'
					AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
					AND	PD10.DF_SPE_ACC_ID = @AccountNumber
			) LN72 
				ON LN10.BF_SSN = LN72.BF_SSN
				AND LN10.LN_SEQ = LN72.LN_SEQ
				AND LN72.SEQ = 1
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
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON LN10.IC_LON_PGM = FMT.Start
				AND FMT.FmtName = '$LNPROG' 
		WHERE 
			PD10.DF_SPE_ACC_ID = @AccountNumber
			AND LN10.LA_CUR_PRI > 0
	END
ELSE
	BEGIN
		SELECT
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [First Disbursement Date],
			'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal Balance],
			CONVERT(VARCHAR(10),LN72.LD_ITR_EFF_BEG,101) AS [SCRA Rate Begin Date],
			CONVERT(VARCHAR(10),LN72.LD_ITR_EFF_END,101) AS [SCRA Rate End Date]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.LC_STA_LON20 = 'A'
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
			INNER JOIN
			(	--Adding Active Interest Rate
				SELECT
					LN72.BF_SSN, 
					LN72.LN_SEQ,
					LN72.LD_ITR_EFF_BEG,
					LN72.LD_ITR_EFF_END,
					ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ, LN72.LD_ITR_EFF_BEG, LN72.LD_ITR_EFF_END ORDER BY LD_STA_LON72 DESC) AS SEQ
				FROM
					LN72_INT_RTE_HST LN72
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN72.BF_SSN
						AND LN20.LN_SEQ = LN72.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
						AND LN20.LC_EDS_TYP = 'M' --Coborrower
					INNER JOIN PD10_PRS_NME PD10 
						ON PD10.DF_PRS_ID = LN20.LF_EDS
				WHERE
					LN72.LC_STA_LON72 = 'A'
					AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
					AND	PD10.DF_SPE_ACC_ID = @AccountNumber
			) LN72 
				ON LN10.BF_SSN = LN72.BF_SSN
				AND LN10.LN_SEQ = LN72.LN_SEQ
				AND LN72.SEQ = 1
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
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON LN10.IC_LON_PGM = FMT.Start
				AND FMT.FmtName = '$LNPROG' 
		WHERE 
			PD10.DF_SPE_ACC_ID = @AccountNumber
			AND LN10.LA_CUR_PRI > 0
	END
END