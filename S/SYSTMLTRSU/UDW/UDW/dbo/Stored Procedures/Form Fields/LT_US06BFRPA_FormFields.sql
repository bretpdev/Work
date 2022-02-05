﻿CREATE PROCEDURE [dbo].[LT_US06BFRPA_FormFields]
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US06BFRPA')

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			'$' + CONVERT(VARCHAR(15), SUM(LN60.LA_ACL_RDC_PAY), 1) AS LA_ACL_RDC_PAY
		FROM
			FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
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
				ON LN85.BF_SSN = LN60.BF_SSN
				AND LN85.LN_SEQ = LN60.LN_SEQ 
		WHERE
			FB10.BF_SSN = @BF_SSN
			AND	FB10.LC_FOR_TYP = '31'
			AND	FB10.LC_FOR_STA = 'A'
			AND	FB10.LC_STA_FOR10 = 'A'
			AND	CAST(LN60.LD_FOR_END AS DATE) > CAST(GETDATE() AS DATE)
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP IN ('000','001')
	END
ELSE
	BEGIN
		SELECT DISTINCT
			'$' + CONVERT(VARCHAR(15), SUM(LN60.LA_ACL_RDC_PAY), 1) AS LA_ACL_RDC_PAY
		FROM
			FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
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
				ON LN85.BF_SSN = LN60.BF_SSN
				AND LN85.LN_SEQ = LN60.LN_SEQ 
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN60.BF_SSN
				AND LN20.LN_SEQ = LN60.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M' --Coborrower
		WHERE
			LN20.LF_EDS = @BF_SSN
			AND FB10.LC_FOR_TYP = '31'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND CAST(LN60.LD_FOR_END AS DATE) > CAST(GETDATE() AS DATE)
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP IN ('000','001')
	END
END