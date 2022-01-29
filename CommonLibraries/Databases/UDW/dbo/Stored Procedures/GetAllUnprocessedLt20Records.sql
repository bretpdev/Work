﻿

CREATE pROCEDURE [dbo].[GetAllUnprocessedLt20Records]
	
AS
	
SELECT DISTINCT
		LT20.DF_SPE_ACC_ID,
		LT20.RM_APL_PGM_PRC,
		LT20.RT_RUN_SRT_DTS_PRC,
		LT20.RN_SEQ_LTR_CRT_PRC, 
		LT20.RM_DSC_LTR_PRC,
		LT20.RF_SBJ_PRC,
		CASE
			WHEN DW01.BF_SSN IS NULL THEN 0
			ELSE 1
		END AS InvalidLoanStatus,
		CASE
			WHEN LTR.LF_ATY_RCP != LT20.RF_SBJ_PRC THEN LTR.LF_ATY_RCP
			ELSE ''
		END AS EndorsersSsn,
		LT20.OnEcorr,
		ISNULL(PH05.DX_CNC_EML_ADR, 'ecorr@utahsbr.edu') AS EmailAddress,
		sr.Recipient,
		LT20.PrintedAt,
		LT20.EcorrDocumentCreatedAt
	FROM
		UDW..LT20_LTR_REQ_PRC LT20
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID
		INNER JOIN
		(
			SELECT
				LT20.DF_SPE_ACC_ID,
				LT20.RM_APL_PGM_PRC,
				LT20.RT_RUN_SRT_DTS_PRC,
				LT20.RN_SEQ_LTR_CRT_PRC,
				LT20.RN_SEQ_REC_PRC,
				LT20.RX_REQ_ARA_1_PRC
			FROM
				UDW..LT20_LTR_REQ_PRC LT20
				INNER JOIN
				(
					SELECT
						LT20.DF_SPE_ACC_ID,
						LT20.RM_APL_PGM_PRC,
						LT20.RT_RUN_SRT_DTS_PRC,
						LT20.RN_SEQ_LTR_CRT_PRC,
						MIN(LT20.RN_SEQ_REC_PRC) AS RN_SEQ_REC_PRC
					FROM
						UDW..LT20_LTR_REQ_PRC LT20
					GROUP BY
						LT20.DF_SPE_ACC_ID,
						LT20.RM_APL_PGM_PRC,
						LT20.RT_RUN_SRT_DTS_PRC,
						LT20.RN_SEQ_LTR_CRT_PRC
				) MIN_REC
					ON MIN_REC.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID
					AND MIN_REC.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
					AND MIN_REC.RT_RUN_SRT_DTS_PRC = LT20.RT_RUN_SRT_DTS_PRC
					AND MIN_REC.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
					AND MIN_REC.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		) LETTER_TEXT
			ON LETTER_TEXT.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID
			AND LETTER_TEXT.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
			AND LETTER_TEXT.RT_RUN_SRT_DTS_PRC = LT20.RT_RUN_SRT_DTS_PRC
			AND LETTER_TEXT.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
			AND LETTER_TEXT.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		LEFT JOIN ULS..SystemLetterExclusions SLE
			ON SLE.LetterId = LT20.RM_DSC_LTR_PRC
		LEFT JOIN UDW..PH05_CNC_EML PH05
			ON PH05.DF_SPE_ID = LT20.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				BF_SSN
			FROM
				UDW..DW01_DW_CLC_CLU
			WHERE
				WC_DW_LON_STA IN ('17','19','21')
		) DW01
		ON DW01.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN
		(
			SELECT
				AY10.BF_SSN,
				AY10.LF_ATY_RCP,
				MST_RCT_ARC.PF_LTR
			FROM
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN 
				(
					SELECT	
						AY10.BF_SSN,
						AY10.PF_REQ_ACT,
						AC11.PF_LTR,
						MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ
					FROM
						UDW..AY10_BR_LON_ATY AY10
						INNER JOIN UDW..AC11_ACT_REQ_LTR AC11
							ON AC11.PF_REQ_ACT = AY10.PF_REQ_ACT
					WHERE
						DATEDIFF(DAY, AY10.LF_LST_DTS_AY10, GETDATE()) <= 14
					GROUP BY
						AY10.BF_SSN,
						AY10.PF_REQ_ACT,
						AC11.PF_LTR
				)MST_RCT_ARC
					ON MST_RCT_ARC.BF_SSN = AY10.BF_SSN
					AND MST_RCT_ARC.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		)LTR
			ON LTR.BF_SSN = LT20.RF_SBJ_PRC
			AND LTR.PF_LTR = LT20.RM_DSC_LTR_PRC
		LEFT JOIN ULS.[dbo].SpecialLetterRecipients SR
		on SR.LetterId = lt20.RM_DSC_LTR_PRC
	WHERE
		((LT20.PrintedAt IS NULL AND LT20.OnEcorr = 0) OR (LT20.EcorrDocumentCreatedAt IS NULL))
		AND LT20.InactivatedAt IS NULL
		and LT20.RI_LTR_REQ_DEL_PRC = 'N'
		AND SLE.LetterId IS NULL

RETURN 0