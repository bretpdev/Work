﻿CREATE PROCEDURE [dbo].[LT_US08B48PMR_Loans]
	@LetterId VARCHAR(10),
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)
--R48 benefit cant apply to Consol Loans per Jesse.  Coborrower logic not needed as a result
IF @IsCoborrower = 0
	BEGIN
		SELECT
			LN10.LN_SEQ AS LN_SEQ,
			COALESCE(FT.label, LN10.IC_LON_PGM, '') AS LoanProgram,
			CASE WHEN OW10.IM_OWN_SHO IN ('UHEAA','UTAH SBR','TILP') THEN 'UHEAA'
				 WHEN OW10.IM_OWN_SHO IN ('COMPLETE STUDENT LN') THEN 'Complete Student Loans'
				 ELSE ISNULL(OW10.IM_OWN_SHO,'')
			END AS Current_Owner,
			CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) AS Date_Disbursed
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
			LEFT JOIN OW10_OWN OW10
				ON LN10.LF_LON_CUR_OWN = OW10.IF_OWN
			LEFT JOIN FormatTranslation FT
				ON LN10.IC_LON_PGM = FT.Start
				AND FT.FmtName = '$LNPROG'
		WHERE 
			LN10.BF_SSN = @BF_SSN
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
		ORDER BY
			LN10.LN_SEQ
	END
END
