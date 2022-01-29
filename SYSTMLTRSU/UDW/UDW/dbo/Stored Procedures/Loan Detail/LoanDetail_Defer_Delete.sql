﻿CREATE PROCEDURE [dbo].[LoanDetail_Defer_Delete]
	@LetterId VARCHAR(10),
	@BF_SSN  VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT 
			CASE WHEN Defer.LC_DFR_STA = 'D' THEN 'Denied'
                 WHEN Defer.LC_STA_LON50 = 'I' then 'Delete'
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
			LN10_LON LN10
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
					LC_DFR_STA,
					LC_STA_LON50,
					LN50.BF_SSN,
					LN50.LN_SEQ
				FROM
					LN50_BR_DFR_APV LN50
					INNER JOIN DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					INNER JOIN 
					(
						SELECT
							MAX(LN50.LD_STA_LON50) AS MaxLN50,
							LN50.BF_SSN,
							LN50.LN_SEQ
						FROM
							LN50_BR_DFR_APV LN50
						GROUP BY
							LN50.BF_SSN,
							LN50.LN_SEQ
					) LN50Max
						ON LN50.BF_SSN = LN50Max.BF_SSN
						AND LN50.LN_SEQ = LN50Max.LN_SEQ
						AND LN50Max.MaxLN50 = LN50.LD_STA_LON50
				WHERE
					DF10.LC_DFR_STA = 'A'
			) Defer
				ON Defer.BF_SSN = LN10.BF_SSN
				AND Defer.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FTD
				ON FTD.Start = Defer.LC_DFR_TYP
				AND FTD.FmtName = '$DEFSTA'
			LEFT JOIN FormatTranslation FT
				ON FT.Start = LN10.IC_LON_PGM
				AND FT.FmtName = '$LNPROG'
		WHERE
			LN10.BF_SSN = @BF_SSN
		ORDER BY 
			LN10.LN_SEQ
	END
ELSE
	BEGIN
		SELECT 
			CASE WHEN Defer.LC_DFR_STA = 'D' THEN 'Denied'
                 WHEN Defer.LC_STA_LON50 = 'I' then 'Delete'
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
			LN10_LON LN10
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M' --Coborrower
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
					LC_DFR_STA,
					LC_STA_LON50,
					LN50.BF_SSN,
					LN50.LN_SEQ
				FROM
					LN50_BR_DFR_APV LN50
					INNER JOIN DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					INNER JOIN 
					(
						SELECT
							MAX(LN50.LD_STA_LON50) AS MaxLN50,
							LN50.BF_SSN,
							LN50.LN_SEQ
						FROM
							LN50_BR_DFR_APV LN50
						GROUP BY
							LN50.BF_SSN,
							LN50.LN_SEQ
					) LN50Max
						ON LN50.BF_SSN = LN50Max.BF_SSN
						AND LN50.LN_SEQ = LN50Max.LN_SEQ
						AND LN50Max.MaxLN50 = LN50.LD_STA_LON50
				WHERE
					DF10.LC_DFR_STA = 'A'
			) Defer
				ON Defer.BF_SSN = LN10.BF_SSN
				AND Defer.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FTD
				ON FTD.Start = Defer.LC_DFR_TYP
				AND FTD.FmtName = '$DEFSTA'
			LEFT JOIN FormatTranslation FT
				ON FT.Start = LN10.IC_LON_PGM
				AND FT.FmtName = '$LNPROG'
		WHERE
			LN20.LF_EDS = @BF_SSN
		ORDER BY 
			LN10.LN_SEQ
	END
END