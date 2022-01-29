﻿CREATE PROCEDURE [dbo].[LT_US10OTCUR4_FormFields]
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US10OTCUR4')
IF @IsCoborrower = 0
	BEGIN
		SELECT 
			SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00)) AS TotalBal
		FROM
			LN10_LON LN10
			INNER JOIN DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
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
		WHERE
			LN10.BF_SSN = @BF_SSN
		GROUP BY
			LN10.BF_SSN
	END
ELSE
	BEGIN
		SELECT 
			SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00)) AS TotalBal
		FROM
			LN10_LON LN10
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
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
		WHERE
			LN20.LF_EDS = @BF_SSN
		GROUP BY
			LN10.BF_SSN
	END
END