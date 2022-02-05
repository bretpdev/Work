CREATE PROCEDURE [repayterm].[GetRepaymentData]
	@AccountIdentifier VARCHAR(10)
AS
	DECLARE @PastDate DATE = CAST('10/07/1998' AS DATE)

	SELECT
		Final.Ssn,
		Final.FirstName,
		Final.MiddleInitial,
		Final.LastName,
		Final.HasNewLoan,
		Final.HasSC,
		Final.HasExtendedLoans,
		SUM(Final.WeightedRate) AS WeightedRate,
		Final.PayoffDate
	FROM
		(
			SELECT
				DC02.BF_SSN AS Ssn,
				RTRIM(PD01.DM_PRS_1) AS FirstName,
				RTRIM(PD01.DM_PRS_MID) AS MiddleInitial,
				RTRIM(PD01.DM_PRS_LST) AS LastName,
				CASE WHEN DC02.LF_DFL_CLR = 'WG000022' OR COUNT(NL.BF_SSN) > 0 THEN 1 ELSE 0 END AS HasNewLoan,
				CASE WHEN ClaimId.SC_SSN IS NOT NULL THEN 1 ELSE 0 END AS HasSC,
				CASE WHEN EL.AF_APL_ID IS NOT NULL THEN 1 ELSE 0 END AS HasExtendedLoans,
				CASE WHEN NL.LD_LDR_POF IS NOT NULL THEN MAX(NL.LD_LDR_POF) ELSE NULL END AS PayoffDate,
				DC02.LR_CUR_INT * (DC02.LA_CLM_BAL / DC02.Total) AS WeightedRate,
				DC02.LA_CLM_BAL,
				DC02.LR_CUR_INT,
				DC02.LF_CLM_ID
			FROM
				ODW..PD01_PDM_INF PD01
				INNER JOIN
				(
					SELECT
						D2.BF_SSN,
						D2.AF_APL_ID,
						D2.AF_APL_ID_SFX,
						D2.LA_CLM_BAL,
						D1.LD_LDR_POF,
						D2.LR_CUR_INT,
						SUM(D2.LA_CLM_BAL) OVER (PARTITION BY D2.BF_SSN) AS Total,
						D1.LF_DFL_CLR,
						D1.LF_CLM_ID
					FROM
						ODW..DC02_BAL_INT D2
						INNER JOIN ODW..DC01_LON_CLM_INF D1
							ON D1.BF_SSN = D2.BF_SSN
							AND D1.AF_APL_ID = D2.AF_APL_ID
							AND D1.AF_APL_ID_SFX = D2.AF_APL_ID_SFX
					WHERE
						D1.LC_STA_DC10 = '03'
				) DC02
					ON DC02.BF_SSN = PD01.DF_PRS_ID
				INNER JOIN
				(
					SELECT
						D.BF_SSN,
						D.LF_CLM_ID,
						SC.BF_SSN SC_SSN
					FROM
						ODW..DC01_LON_CLM_INF D
						LEFT JOIN
						( --Borrower has an open specialty claim
							SELECT
								BF_SSN
							FROM
								ODW..DC01_LON_CLM_INF
							WHERE
								LC_PCL_REA NOT IN ('DF','DB')
						) AS SC
							ON SC.BF_SSN = D.BF_SSN
					WHERE
						LC_STA_DC10 = '03' --Must be a defaulted loan
						AND LD_CLM_ASN_DOE IS NULL --Can't be assigned to ED
				) AS ClaimId
					ON ClaimId.BF_SSN = DC02.BF_SSN
					AND ClaimId.LF_CLM_ID = DC02.LF_CLM_ID
				LEFT JOIN
				( --Borrower has a loan that recently defaulted
					SELECT
						BF_SSN,
						LD_LDR_POF
					FROM
						ODW..DC01_LON_CLM_INF
					WHERE
						LC_STA_DC10 = '03'
						AND LD_LDR_POF > CAST(GETDATE() - 60 AS DATE)
				) NL
					ON NL.BF_SSN = DC02.BF_SSN
				LEFT JOIN
				(
					SELECT
						GA11.AF_APL_ID
					FROM
						ODW..GA11_LON_DSB_ATY GA11
						INNER JOIN ODW..DC02_BAL_INT D2
							ON D2.AF_APL_ID = GA11.AF_APL_ID
					WHERE
						AN_DSB_SEQ = 1 --Look at the first sequence
						AND AC_DSB_ADJ = 'A' --Want to look at the actual disbursement
						AND AD_DSB_ADJ < @PastDate --Only pull back records that were before this date
						AND D2.LA_CLM_BAL > 31000
				) EL
					ON EL.AF_APL_ID = DC02.AF_APL_ID
			WHERE
				@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
			GROUP BY
				DC02.BF_SSN,
				PD01.DM_PRS_1,
				PD01.DM_PRS_MID,
				PD01.DM_PRS_LST,
				DC02.LF_DFL_CLR,
				DC02.LR_CUR_INT,
				DC02.LA_CLM_BAL,
				DC02.Total,
				DC02.LF_CLM_ID,
				ClaimId.SC_SSN,
				NL.LD_LDR_POF,
				EL.AF_APL_ID
		) Final
	GROUP BY
		Final.Ssn,
		Final.FirstName,
		Final.MiddleInitial,
		Final.LastName,
		Final.HasNewLoan,
		Final.HasSC,
		Final.HasExtendedLoans,
		Final.PayoffDate