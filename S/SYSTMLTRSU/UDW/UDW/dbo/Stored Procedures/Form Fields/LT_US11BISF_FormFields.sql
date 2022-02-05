CREATE PROCEDURE [dbo].[LT_US11BISF_FormFields]
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US11BISF')

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			LN90.BF_SSN,
			CONVERT(VARCHAR, LN90.[Date], 101) [Date],
			'$' + CONVERT(VARCHAR, CAST(LN90.Amount AS MONEY), 1) [Amount],
			LN90.Reason
		FROM
			LN10_LON LN10
			INNER JOIN 
			(
				SELECT
					LN90.BF_SSN,
					MAX_REV.LD_FAT_EFF AS [Date],
					SUM(ISNULL(LN90.LA_FAT_NSI, 0) + ISNULL(LN90.LA_FAT_LTE_FEE, 0) + ISNULL(LN90.LA_FAT_CUR_PRI, 0)) AS Amount,
					MAX(
						CASE
							WHEN LN90.LC_FAT_REV_REA = '0' THEN 'System Generated'
							WHEN LN90.LC_FAT_REV_REA = '1' THEN 'Bad Check - Insufficient Funds'
							WHEN LN90.LC_FAT_REV_REA = '2' THEN 'Bank Memo - Incorrect Amount'
							WHEN LN90.LC_FAT_REV_REA = '3' THEN 'Incorrect Amount'
							WHEN LN90.LC_FAT_REV_REA = '4' THEN 'Other'
							WHEN LN90.LC_FAT_REV_REA = '5' THEN 'Incorrect Amount - Partial Refund'
							WHEN LN90.LC_FAT_REV_REA = '6' THEN 'Bad Check - Not Endorsed/Wrong Payee'
							WHEN LN90.LC_FAT_REV_REA = '7' THEN 'Incorrect Payee/Full Refund'
							WHEN LN90.LC_FAT_REV_REA = '9' THEN 'Reverse/Reapply to Multiple Borrowers E - Other - Check Voided K - Reversal of Ineligible Principal L - Other'
						END) AS Reason
				FROM
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							MAX(LD_FAT_EFF) AS LD_FAT_EFF
						FROM
							LN90_FIN_ATY LN90
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
								ON LN85.BF_SSN = LN90.BF_SSN
								AND LN85.LN_SEQ = LN90.LN_SEQ 
						WHERE
							LN90.LC_STA_LON90 = 'A'
							AND ISNULL(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
							AND LN90.PC_FAT_TYP = '10'
							AND LN90.BF_SSN = @BF_SSN
						GROUP BY
							LN90.BF_SSN,
							LN90.LN_SEQ
					) MAX_REV
						ON MAX_REV.BF_SSN = LN90.BF_SSN
						AND MAX_REV.LN_SEQ = LN90.LN_SEQ
						AND MAX_REV.LD_FAT_EFF = LN90.LD_FAT_EFF
				WHERE
					LN90.LC_STA_LON90 = 'A'
					AND ISNULL(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
					AND LN90.PC_FAT_TYP = '10'
					and LN90.BF_SSN = @BF_SSN
				GROUP BY
					LN90.BF_SSN,
					MAX_REV.LD_FAT_EFF
			) LN90
				ON LN10.BF_SSN = LN90.BF_SSN
		WHERE
			LN10.BF_SSN = @BF_SSN
	END
ELSE
	BEGIN
		SELECT DISTINCT
			LN90.BF_SSN,
			CONVERT(VARCHAR, LN90.[Date], 101) [Date],
			'$' + CONVERT(VARCHAR, CAST(LN90.Amount AS MONEY), 1) [Amount],
			LN90.Reason
		FROM
			LN10_LON LN10
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN 
			(
				SELECT
					LN90.BF_SSN,
					MAX_REV.LD_FAT_EFF AS [Date],
					SUM(ISNULL(LN90.LA_FAT_NSI, 0) + ISNULL(LN90.LA_FAT_LTE_FEE, 0) + ISNULL(LN90.LA_FAT_CUR_PRI, 0)) AS Amount,
					MAX(
						CASE
							WHEN LN90.LC_FAT_REV_REA = '0' THEN 'System Generated'
							WHEN LN90.LC_FAT_REV_REA = '1' THEN 'Bad Check - Insufficient Funds'
							WHEN LN90.LC_FAT_REV_REA = '2' THEN 'Bank Memo - Incorrect Amount'
							WHEN LN90.LC_FAT_REV_REA = '3' THEN 'Incorrect Amount'
							WHEN LN90.LC_FAT_REV_REA = '4' THEN 'Other'
							WHEN LN90.LC_FAT_REV_REA = '5' THEN 'Incorrect Amount - Partial Refund'
							WHEN LN90.LC_FAT_REV_REA = '6' THEN 'Bad Check - Not Endorsed/Wrong Payee'
							WHEN LN90.LC_FAT_REV_REA = '7' THEN 'Incorrect Payee/Full Refund'
							WHEN LN90.LC_FAT_REV_REA = '9' THEN 'Reverse/Reapply to Multiple Borrowers E - Other - Check Voided K - Reversal of Ineligible Principal L - Other'
						END) AS Reason
				FROM
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							MAX(LD_FAT_EFF) AS LD_FAT_EFF
						FROM
							LN90_FIN_ATY LN90
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
								ON LN85.BF_SSN = LN90.BF_SSN
								AND LN85.LN_SEQ = LN90.LN_SEQ 
							INNER JOIN LN20_EDS LN20
								ON LN20.BF_SSN = LN90.BF_SSN
								AND LN20.LN_SEQ = LN90.LN_SEQ
								AND LN20.LC_STA_LON20 = 'A'
								AND LN20.LC_EDS_TYP = 'M'
						WHERE
							LN90.LC_STA_LON90 = 'A'
							AND ISNULL(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
							AND LN90.PC_FAT_TYP = '10'
							AND LN20.LF_EDS = @BF_SSN
						GROUP BY
							LN90.BF_SSN,
							LN90.LN_SEQ
					) MAX_REV
						ON MAX_REV.BF_SSN = LN90.BF_SSN
						AND MAX_REV.LN_SEQ = LN90.LN_SEQ
						AND MAX_REV.LD_FAT_EFF = LN90.LD_FAT_EFF
				WHERE
					LN90.LC_STA_LON90 = 'A'
					AND ISNULL(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
					AND LN90.PC_FAT_TYP = '10'
				GROUP BY
					LN90.BF_SSN,
					MAX_REV.LD_FAT_EFF
			) LN90
				ON LN10.BF_SSN = LN90.BF_SSN
		WHERE
			LN20.LF_EDS = @BF_SSN
	END
END