--Open and Released Loans:
--LNXX.LA_CUR_PRI > X
--LNXX.LC_STA_LNXX = R

--Split Due Dates: 
--LNXX.LC_STA_LONXX = A
--RSXX.LC_STA_RPSTXX = A
--Only include if borrower has different �DD� (MM-DD-CCYY) RSXX.LD_RPS_X_PAY_DU records 

--Active, Approved ACH Loans:
--BRXX.BC_EFT_STA = A
--LNXX.LC_STA_LNXX = A for the same loan(s)
--If multiple LNXX.BN_EFT_SEQ exists for the same loan, chose the max sequence 

USE CDW
GO

SELECT
	XDD.BF_SSN,
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX],
	MAX(CASE WHEN XDD.seq = X THEN XDD.DueDay ELSE NULL END) [DueDayX]
FROM
	( -- create sequence for cross tab
		SELECT DISTINCT
			CDD.BF_SSN,
			CDD.DueDay,
			ROW_NUMBER() OVER (PARTITION BY CDD.BF_SSN ORDER BY CDD.DueDay) [seq]
		FROM
			( -- counted distinct due date
				SELECT
					DD.BF_SSN,
					DD.DueDay,
					COUNT(*) OVER (PARTITION BY DD.BF_SSN)  [DD_count]
				FROM
					( -- distinct list of SSNs and relevant Xst pay due dates
						SELECT DISTINCT
							RSXX.BF_SSN,
							DATEPART(DD, RSXX.LD_RPS_X_PAY_DU) [DueDay]
						FROM
							RSXX_BR_RPD RSXX
							INNER JOIN LNXX_LON LNXX ON LNXX.BF_SSN = RSXX.BF_SSN
							INNER JOIN LNXX_LON_RPS LNXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
							INNER JOIN OPENQUERY
							( -- no data in local table
								LEGEND,
								'
									SELECT
										BRXX.BF_SSN
									FROM
										PKUB.BRXX_BR_EFT BRXX
									WHERE
										BRXX.BC_EFT_STA = ''A''
								'
							) BRXX ON BRXX.BF_SSN = RSXX.BF_SSN
							INNER JOIN 
							(  -- sequenced active LNXX records
								SELECT
									LNXX.BF_SSN,
									LNXX.LN_SEQ,
									ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LNXX.BN_EFT_SEQ DESC) [Seq] -- order so max sequence has sequence X
								FROM
									LNXX_EFT_TO_LON LNXX
								WHERE
									LNXX.LC_STA_LNXX = 'A'
							) LNXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.Seq = X -- max sequence only
						WHERE
							RSXX.LC_STA_RPSTXX = 'A'
							AND
							LNXX.LA_CUR_PRI > X
							AND
							LNXX.LC_STA_LONXX = 'R'
					) DD
			) CDD
		WHERE
			CDD.DD_count > X -- include only those borrowers who have more than one due date
	) XDD
GROUP BY
	XDD.BF_SSN
ORDER BY
	XDD.BF_SSN
