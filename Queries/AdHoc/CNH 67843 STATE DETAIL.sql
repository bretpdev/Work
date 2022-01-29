

-- AZ
--CA
--CO
--CT
--DC

--IL
--ME
--MD
--MA
--NJ
--NY
--OR
--RI
--SC
--VT
--VA
--WA


SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @DATA TABLE (QUESTION VARCHAR(XXX), DATEX VARCHAR(XX), DATEX VARCHAR(XX))
DECLARE @BWRS TABLE (BF_SSN VARCHAR(X), DF_SPE_ACC_ID VARCHAR(XX), LOAN_BALANCE DECIMAL(XX,X) )
INSERT INTO @BWRS
SELECT DISTINCT
	PDXX.DF_PRS_ID,
	PDXX.DF_SPE_ACC_ID,
	SUM(LA_CUR_PRI) AS ACCOUNT_BALANCE
FROM
	UDW..PDXX_PRS_NME PDXX
	INNER JOIN UDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN UDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DC_DOM_ST = 'WA'
	AND PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_ADR = 'L'
	and pdXX.DD_VER_ADR < 'XX/XX/XXXX' --adding this for the re-run  the query was run for the first time on XX/XX
GROUP BY
	PDXX.DF_PRS_ID,
	PDXX.DF_SPE_ACC_ID



SELECT 
	isnull(ba.df_spe_acc_id, bs.df_spe_acc_id) as account_number,
	isnull(ba.LOAN_BALANCE, bs.LOAN_BALANCE) as LOAN_BALANCE
FROM
	NobleCalls..NobleCallHistory NCH
	LEFT JOIN @BWRS BA
		ON BA.DF_SPE_ACC_ID = NCH.AccountIdentifier
	LEFT JOIN @BWRS BS
		ON BS.BF_SSN = NCH.AccountIdentifier
WHERE
	RegionId = X
	AND DeletedAt IS NULL
	AND CreatedAt BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND IsInbound = X
	AND (BA.DF_SPE_ACC_ID IS NOT NULL OR BS.BF_SSN IS NOT NULL)


SELECT 
	isnull(ba.df_spe_acc_id, bs.df_spe_acc_id) as account_number,
	isnull(ba.LOAN_BALANCE, bs.LOAN_BALANCE) as LOAN_BALANCE
FROM
	NobleCalls..NobleCallHistory NCH
	LEFT JOIN @BWRS BA
		ON BA.DF_SPE_ACC_ID = NCH.AccountIdentifier
	LEFT JOIN @BWRS BS
		ON BS.BF_SSN = NCH.AccountIdentifier
WHERE
	RegionId = X
	AND DeletedAt IS NULL
	AND CreatedAt BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND IsInbound = X
	AND (BA.DF_SPE_ACC_ID IS NOT NULL OR BS.BF_SSN IS NOT NULL)




SELECT DISTINCT
	B.account_number,
	BS.LOAN_BALANCE,
	A.application_id
FROM
	IncomeBasedRepaymentUheaa..Applications A
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON l.application_id = A.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN @BWRS BS
		ON B.SSN = BS.BF_SSN
WHERE
	ACTIVE = X
	AND A.created_at BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

SELECT distinct
	B.account_number,
	BS.LOAN_BALANCE,
	A.application_id
FROM
	IncomeBasedRepaymentUheaa..Applications A
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON l.application_id = A.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN @BWRS BS
		ON B.SSN = BS.BF_SSN
WHERE
	ACTIVE = X
	AND A.created_at BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'


	SELECT DISTINCT
		B.account_number,
	BS.LOAN_BALANCE,
	APP.application_id
	FROM
		IncomeBasedRepaymentUheaa..Applications App
		inner join IncomeBasedRepaymentUheaa..Repayment_Plan_Selected RPS
			on RPS.application_id = App.application_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History HIS
			ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Substatus SUB
			ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status STA
			ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
		inner join
		(
			select	
				h.repayment_plan_type_id,
				max(h.created_at) as created_at

			from
				IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History h
			group by
				h.repayment_plan_type_id
		) max_date
			on his.repayment_plan_type_id = max_date.repayment_plan_type_id
			and his.created_at = max_date.created_at
		INNER JOIN IncomeBasedRepaymentUheaa..Loans L
			ON l.application_id = APP.application_id
		INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
			ON B.borrower_id = L.borrower_id
		INNER JOIN @BWRS BS
			ON B.SSN = BS.BF_SSN

	WHERE
		APP.created_at  BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
		AND Active = X
		AND sta.repayment_plan_type_status = 'APPROVED'

	SELECT distinct
		B.account_number,
	BS.LOAN_BALANCE,
	APP.application_id
	FROM
		IncomeBasedRepaymentUheaa..Applications App
		inner join IncomeBasedRepaymentUheaa..Repayment_Plan_Selected RPS
			on RPS.application_id = App.application_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History HIS
			ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Substatus SUB
			ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
		INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status STA
			ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
		inner join
		(
			select	
				h.repayment_plan_type_id,
				max(h.created_at) as created_at

			from
				IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History h
			group by
				h.repayment_plan_type_id
		) max_date
			on his.repayment_plan_type_id = max_date.repayment_plan_type_id
			and his.created_at = max_date.created_at
		INNER JOIN IncomeBasedRepaymentUheaa..Loans L
			ON l.application_id = APP.application_id
		INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
			ON B.borrower_id = L.borrower_id
		INNER JOIN @BWRS BS
			ON B.SSN = BS.BF_SSN

	WHERE
		APP.created_at  BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
		AND Active = X
		AND sta.repayment_plan_type_status = 'APPROVED'

INSERT INTO @DATA
SELECT
	'Total Number of commercially-held FFELP loans being serviced?',
	*
FROM
(
SELECT
	COUNT(*) AS F
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'
)P
CROSS JOIN
(
SELECT
	COUNT(*) AS S
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'

) CJ

SELECT
	BS.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LNXX.LN_SEQ AS LOAN_SEQ,
	LNXX.LA_CUR_PRI AS LOAN_BALANCE
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'

SELECT
	BS.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LNXX.LN_SEQ AS LOAN_SEQ,
	LNXX.LA_CUR_PRI AS LOAN_BALANCE
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'

SELECT DISTINCT
	BS.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	BS.LOAN_BALANCE 
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'

SELECT DISTINCT
	BS.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	BS.LOAN_BALANCE 
FROM
	AuditUDW..LNXX_LON_AprXXXX LNXX
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'





	SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
	FROM
	(
	SELECT DISTINCT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID,
		MAX(CASE 
			WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
		END) AS DDP
	FROM
		UDW..LNXX_LON_DLQ_HST LNXX
		INNER JOIN @BWRS BS
			ON BS.BF_SSN = LNXX.BF_SSN
	WHERE
		LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
		AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
	GROUP BY
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID
	) DD
	INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
		ON LNXX.BF_SSN = DD.BF_SSN
		AND LNXX.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN XX AND XX
	
		SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
		FROM
		(
		SELECT DISTINCT
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID,
			MAX(CASE 
				WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
			END) AS DDP
		FROM
			UDW..LNXX_LON_DLQ_HST LNXX
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LNXX.BF_SSN
		WHERE
			LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
			AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
		GROUP BY
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID
		) DD
		INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
			ON LNXX.BF_SSN = DD.BF_SSN
			AND LNXX.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN XX AND XX



	SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
	FROM
	(
	SELECT DISTINCT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID,
		MAX(CASE 
			WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
		END) AS DDP
	FROM
		UDW..LNXX_LON_DLQ_HST LNXX
		INNER JOIN @BWRS BS
			ON BS.BF_SSN = LNXX.BF_SSN
	WHERE
		LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
		AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
	GROUP BY
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID
	) DD
	INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
		ON LNXX.BF_SSN = DD.BF_SSN
		AND LNXX.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN XX AND XX
	
		SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
		FROM
		(
		SELECT DISTINCT
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID,
			MAX(CASE 
				WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
			END) AS DDP
		FROM
			UDW..LNXX_LON_DLQ_HST LNXX
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LNXX.BF_SSN
		WHERE
			LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
			AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
		GROUP BY
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID
		) DD
		INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
			ON LNXX.BF_SSN = DD.BF_SSN
			AND LNXX.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN XX AND XX
	SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
	FROM
	(
	SELECT DISTINCT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID,
		MAX(CASE 
			WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
		END) AS DDP
	FROM
		UDW..LNXX_LON_DLQ_HST LNXX
		INNER JOIN @BWRS BS
			ON BS.BF_SSN = LNXX.BF_SSN
	WHERE
		LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
		AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
	GROUP BY
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		BS.DF_SPE_ACC_ID
	) DD
	INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
		ON LNXX.BF_SSN = DD.BF_SSN
		AND LNXX.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP  >= XX
	
		SELECT DISTINCT
		DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
		DD.LN_SEQ,
		LNXX.LA_CUR_PRI AS LOAN_BALANCE
		FROM
		(
		SELECT DISTINCT
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID,
			MAX(CASE 
				WHEN LD_DLQ_MAX < 'XX/XX/XXXX' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, 'XX/XX/XXXX')
			END) AS DDP
		FROM
			UDW..LNXX_LON_DLQ_HST LNXX
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LNXX.BF_SSN
		WHERE
			LD_DLQ_OCC < 'XX/XX/XXXX' --past due before the cutoff date 
			AND LD_DLQ_MAX > 'XX/XX/XXXX' --past due after
		GROUP BY
			LNXX.bf_ssn,
			LNXX.ln_seq,
			BS.DF_SPE_ACC_ID
		) DD
		INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
			ON LNXX.BF_SSN = DD.BF_SSN
			AND LNXX.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP  >= XX

		SELECT DISTINCT
			DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
			LOAN_BALANCE
		FROM
			UDW..LNXX_LON LNXX
			INNER JOIN
			(
				SELECT 
					FBXX.LC_FOR_TYP,
					LNXX.LD_FOR_BEG,
					LNXX.LD_FOR_END,
					FBXX.LD_CRT_REQ_FOR,
					FBXX.LF_LST_DTS_FBXX,
					FBXX.LC_FOR_STA,
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					BS.DF_SPE_ACC_ID,
					LOAN_BALANCE
				FROM
					UDW..FBXX_BR_FOR_REQ FBXX
					INNER JOIN UDW..LNXX_BR_FOR_APV LNXX
						ON LNXX.BF_SSN = FBXX.BF_SSN
						AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FBXX.BF_SSN
				WHERE
					LNXX.LC_STA_LONXX = 'A'
					AND FBXX.LC_STA_FORXX = 'A'
					--AND FBXX.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LNXX.BF_SSN
				AND Forb.LN_SEQ = LNXX.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

		SELECT distinct
			DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
			LOAN_BALANCE
		FROM
			UDW..LNXX_LON LNXX
			INNER JOIN
			(
				SELECT 
					FBXX.LC_FOR_TYP,
					LNXX.LD_FOR_BEG,
					LNXX.LD_FOR_END,
					FBXX.LD_CRT_REQ_FOR,
					FBXX.LF_LST_DTS_FBXX,
					FBXX.LC_FOR_STA,
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					DF_SPE_ACC_ID,
					LOAN_BALANCE
				FROM
					UDW..FBXX_BR_FOR_REQ FBXX
					INNER JOIN UDW..LNXX_BR_FOR_APV LNXX
						ON LNXX.BF_SSN = FBXX.BF_SSN
						AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FBXX.BF_SSN
				WHERE
					LNXX.LC_STA_LONXX = 'A'
					AND FBXX.LC_STA_FORXX = 'A'
					--AND FBXX.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LNXX.BF_SSN
				AND Forb.LN_SEQ = LNXX.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'



SELECT distinct
	BS.DF_SPE_ACC_ID,
	BS.LOAN_BALANCE
FROM
	AuditUDW..DWXX_DW_CLC_CLU_AprXXXX DWXX
	INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DWXX.BF_SSN
WHERE
	WC_DW_LON_STA = XX

SELECT distinct
	BS.DF_SPE_ACC_ID,
	BS.LOAN_BALANCE
FROM
	AuditUDW..DWXX_DW_CLC_CLU_AprXXXX DWXX
	INNER JOIN AuditUDW..LNXX_LON_AprXXXX LNXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DWXX.BF_SSN
WHERE
	WC_DW_LON_STA = XX
