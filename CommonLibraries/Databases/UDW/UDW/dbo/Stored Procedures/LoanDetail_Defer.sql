﻿
CREATE PROCEDURE [dbo].[LoanDetail_Defer]
	@LetterId varchar(10),
	@BF_SSN  CHAR(9)
AS

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

SELECT
	CASE 
		WHEN Defer.LC_DFR_STA = 'D' THEN 'Denied'
		WHEN DATEDIFF(DAY, Defer.LD_CRT_REQ_DFR, Defer.LF_LST_DTS_DF10) = 0 THEN 'Add'
		WHEN DATEDIFF(DAY, Defer.LD_CRT_REQ_DFR, Defer.LF_LST_DTS_DF10) != 0 THEN 'Change'
		ELSE 'Delete'
	END AS LetterAction,
	ISNULL(FTD.Label, Defer.LC_DFR_TYP) AS LetterTypeCode,
	CONVERT(VARCHAR(10),Defer.LD_DFR_BEG,101) AS BeginDate,
	CONVERT(VARCHAR(10),Defer.LD_DFR_END,101) AS EndDate,
	LN10.LN_SEQ,
	ISNULL(FT.Label, LN10.IC_LON_PGM) AS LoanProgram
FROM
	UDW..LN10_LON LN10
	LEFT JOIN UDW..FormatTranslation FT
		ON FT.Start = LN10.IC_LON_PGM
	INNER JOIN
	(
		SELECT
			LN85.BF_SSN,
			LN85.LN_SEQ
		FROM
			UDW..LN85_LON_ATY LN85
			INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM 
					UDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT = @PF_REQ_ACT
				GROUP BY
					AY10.BF_SSN
			)AY10
				ON AY10.BF_SSN = LN85.BF_SSN
				AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
	)LN85
		ON LN85.BF_SSN = LN10.BF_SSN
		AND LN85.LN_SEQ = LN10.LN_SEQ
	INNER JOIN
	(
		SELECT 
			DF10.LC_DFR_TYP,
			LN50.LD_DFR_BEG,
			LN50.LD_DFR_END,
			DF10.LD_CRT_REQ_DFR,
			DF10.LF_LST_DTS_DF10,
			DF10.LC_DFR_STA,
			LN50.BF_SSN,
			LN50.LN_SEQ
		FROM
			UDW..LN50_BR_DFR_APV LN50
			INNER JOIN UDW..DF10_BR_DFR_REQ DF10
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
			INNER JOIN 
			(
				SELECT
					MAX(LN50.LD_STA_LON50) AS MaxLN50,
					BF_SSN,
					LN_SEQ
				FROM
					UDW..LN50_BR_DFR_APV LN50
				GROUP BY
					LN50.BF_SSN,
					LN50.LN_SEQ
			) LN50Max
				ON LN50.BF_SSN = LN50Max.BF_SSN
				AND LN50.LN_SEQ = LN50Max.LN_SEQ
				AND LN50Max.MaxLN50 = LN50.LD_STA_LON50
		WHERE
			LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			--AND DF10.LC_DFR_STA = 'A' removing this to account for denied requests
	) Defer
		ON Defer.BF_SSN = LN10.BF_SSN
		AND Defer.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..FormatTranslation FTD
		ON FTD.Start = Defer.LC_DFR_TYP
		AND FTD.FmtName = '$DEFSTA'

WHERE
	LN10.BF_SSN = @BF_SSN
ORDER BY 
	LN10.LN_SEQ