
CREATE PROCEDURE [dbo].[LT_US11BISF_FormFields]
	@BF_SSN  CHAR(9)
AS


DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US11BISF')

SELECT DISTINCT
	LN90.BF_SSN,
	CONVERT(varchar, LN90.[Date], 101) [Date],
	'$' + CONVERT(varchar, CAST(LN90.Amount AS money), 1) [Amount],
	LN90.Reason
FROM
	UDW..LN10_LON LN10
	INNER JOIN 
		(
			SELECT
				LN90.BF_SSN,
				MAX_REV.LD_FAT_EFF as [Date],
				SUM(COALESCE(LN90.LA_FAT_NSI, 0) + COALESCE(LN90.LA_FAT_LTE_FEE, 0) + COALESCE(LN90.LA_FAT_CUR_PRI, 0)) AS Amount,
				MAX(
					CASE
						WHEN LN90.LC_FAT_REV_REA = '0' THEN 'System Generated'
						WHEN LN90.LC_FAT_REV_REA = '1' THEN 'Bad Check – Insufficient Funds'
						WHEN LN90.LC_FAT_REV_REA = '2' THEN 'Bank Memo – Incorrect Amount'
						WHEN LN90.LC_FAT_REV_REA = '3' THEN 'Incorrect Amount'
						WHEN LN90.LC_FAT_REV_REA = '4' THEN 'Other'
						WHEN LN90.LC_FAT_REV_REA = '5' THEN 'Incorrect Amount – Partial Refund'
						WHEN LN90.LC_FAT_REV_REA = '6' THEN 'Bad Check – Not Endorsed/Wrong Payee'
						WHEN LN90.LC_FAT_REV_REA = '7' THEN 'Incorrect Payee/Full Refund'
						WHEN LN90.LC_FAT_REV_REA = '9' THEN 'Reverse/Reapply  to Multiple Borrowers E – Other – Check Voided K – Reversal of Ineligible Principal L  - Other'
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
							ON LN85.BF_SSN = LN90.BF_SSN
							AND LN85.LN_SEQ = LN90.LN_SEQ
					where
						LN90.LC_STA_LON90 = 'A'
						AND COALESCE(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
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
				AND COALESCE(LN90.LC_FAT_REV_REA, '') NOT IN ('','0')
				AND LN90.PC_FAT_TYP = '10'
				and LN90.BF_SSN = @BF_SSN
			GROUP BY
				LN90.BF_SSN,
				MAX_REV.LD_FAT_EFF
		) LN90
			ON LN10.BF_SSN = LN90.BF_SSN
WHERE
	LN10.BF_SSN = @BF_SSN

RETURN 0