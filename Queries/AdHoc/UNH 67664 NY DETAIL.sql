
DECLARE @DATA TABLE (QUESTION VARCHAR(200), DATE1 VARCHAR(40), DATE2 VARCHAR(40))
DECLARE @BWRS TABLE (BF_SSN VARCHAR(9), DF_SPE_ACC_ID VARCHAR(10) )
INSERT INTO @BWRS
SELECT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID
	--SUM(LA_CUR_PRI) AS LOAN_BALANCE
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
WHERE
	PD30.DC_DOM_ST = 'NY'
	AND PD30.DI_VLD_ADR = 'Y'
	AND PD30.DC_ADR = 'L'

--GROUP BY
--	PD10.DF_PRS_ID,
--	PD10.DF_SPE_ACC_ID


--'What is the number of phone calls received during the time frame for Private loans, commercially-held loans, and Perkins loans?'
SELECT
	*
FROM
(
SELECT 
	--isnull(ba.df_spe_acc_id, bs.df_spe_acc_id) as account_number,
	--isnull(ba.LOAN_BALANCE, bs.LOAN_BALANCE) as LOAN_BALANCE,
	--NobleCallHistoryId
	count(*) as c
FROM
	NobleCalls..NobleCallHistory NCH
	LEFT JOIN @BWRS BA
		ON BA.DF_SPE_ACC_ID = NCH.AccountIdentifier
	LEFT JOIN @BWRS BS
		ON BS.BF_SSN = NCH.AccountIdentifier
WHERE
	RegionId = 2
	AND DeletedAt IS NULL
	AND CreatedAt BETWEEN '03/01/2020' AND '05/15/2020'
	AND IsInbound = 1
	AND (BA.DF_SPE_ACC_ID IS NOT NULL OR BS.BF_SSN IS NOT NULL)
) P


SELECT 
	isnull(ba.df_spe_acc_id, bs.df_spe_acc_id) as account_number,
	--isnull(ba.LOAN_BALANCE, bs.LOAN_BALANCE) as LOAN_BALANCE,
	NobleCallHistoryId
FROM
	NobleCalls..NobleCallHistory NCH
	LEFT JOIN @BWRS BA
		ON BA.DF_SPE_ACC_ID = NCH.AccountIdentifier
	LEFT JOIN @BWRS BS
		ON BS.BF_SSN = NCH.AccountIdentifier
WHERE
	RegionId = 2
	AND DeletedAt IS NULL
	AND CreatedAt BETWEEN '03/01/2019' AND '05/15/2019'
	AND IsInbound = 1
	AND (BA.DF_SPE_ACC_ID IS NOT NULL OR BS.BF_SSN IS NOT NULL)
order by account_number
INSERT INTO @DATA
SELECT
	'What is the total number of income-driven repayment applications approved for Private loans, commercially-held loans, and Perkins loans?',
	*
FROM
(
SELECT
	COUNT(*) AS F
FROM
	IncomeBasedRepaymentUheaa..Applications A
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON l.application_id = A.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN @BWRS BS
		ON B.SSN = BS.BF_SSN
WHERE
	ACTIVE = 1
	AND A.created_at BETWEEN '03/01/2020' AND '05/15/2020'
) P
CROSS JOIN
(
SELECT
	COUNT(*) AS S
FROM
	IncomeBasedRepaymentUheaa..Applications A
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON l.application_id = A.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN @BWRS BS
		ON B.SSN = BS.BF_SSN
WHERE
	ACTIVE = 1
	AND A.created_at BETWEEN '03/01/2019' AND '05/15/2019'
) CJ
INSERT INTO @DATA
SELECT
	'What is the total number of income-driven repayment applications approved for Private loans, commercially-held loans, and Perkins loans?',
	*
FROM
(
	SELECT
		COUNT(DISTINCT APP.application_id) AS F
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
		APP.created_at  BETWEEN '03/01/2020' AND '05/15/2020'
		AND Active = 1
		AND sta.repayment_plan_type_status = 'APPROVED'
) P
CROSS JOIN
(
	SELECT
		COUNT(DISTINCT APP.application_id) AS S
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
		APP.created_at  BETWEEN '03/01/2019' AND '05/15/2019'
		AND Active = 1
		AND sta.repayment_plan_type_status = 'APPROVED'
) CJ
INSERT INTO @DATA
SELECT
	'Total Number of commercially-held FFELP loans being serviced?',
	*
FROM
(
SELECT
	COUNT(*) AS F
FROM
	AuditUDW..LN10_LON_Apr2020 LN10
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LN10.BF_SSN
WHERE
	LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'
)P
CROSS JOIN
(
SELECT
	COUNT(*) AS S
FROM
	AuditUDW..LN10_LON_Apr2019 LN10
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LN10.BF_SSN
WHERE
	LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'

) CJ
INSERT INTO @DATA
SELECT
	'Total dollar amount of commercially-held FFELP loans being serviced?',
	*
FROM
(
SELECT
	SUM(LA_CUR_PRI) AS F
FROM
	AuditUDW..LN10_LON_Apr2020 LN10
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LN10.BF_SSN
WHERE
	LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'
)P
CROSS JOIN
(
SELECT
	SUM(LA_CUR_PRI) AS S
FROM
	AuditUDW..LN10_LON_Apr2019 LN10
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = LN10.BF_SSN
WHERE
	LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'

) CJ

INSERT INTO @DATA
SELECT
	'Total number of commercially-held FFELP loans that are 30 days delinquent?', 
	* 
FROM
(
	SELECT
		count(*) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.BF_SSN,
		LN16.LN_SEQ,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
			ON BS.BF_SSN = LN16.BF_SSN
	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.BF_SSN,
		LN16.LN_SEQ
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN 30 AND 59
	) P
	CROSS JOIN 
	(
		SELECT
			count(*) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			LN16.ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			LN16.ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN 30 AND 59
	) AS CJ

INSERT INTO @DATA	
SELECT
	'Total dollar amount of commercially-held FFELP loans that are 30 days delinquent?', 
	* 
FROM
(
	SELECT
		SUM(LA_CUR_PRI) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.bf_ssn,
		ln_seq,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN

	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.bf_ssn,
		ln_seq
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN 30 AND 59
	) P
	CROSS JOIN 
	(
		SELECT
			SUM(LA_CUR_PRI) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN 30 AND 59
	) AS CJ

INSERT INTO @DATA
SELECT
	'Total number of commercially-held FFELP loans that are 60 days delinquent?', 
	* 
FROM
(
	SELECT
		count(*) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.bf_ssn,
		ln_seq,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.bf_ssn,
		ln_seq
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN 60 AND 89
	) P
	CROSS JOIN 
	(
		SELECT
			count(*) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN 60 AND 89
	) AS CJ

INSERT INTO @DATA	
SELECT
	'Total dollar amount of commercially-held FFELP loans that are 60 days delinquent?', 
	* 
FROM
(
	SELECT
		SUM(LA_CUR_PRI) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.bf_ssn,
		ln_seq,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.bf_ssn,
		ln_seq
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP BETWEEN 60 AND 89
	) P
	CROSS JOIN 
	(
		SELECT
			SUM(LA_CUR_PRI) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP BETWEEN 60 AND 89
	) AS CJ

INSERT INTO @DATA
SELECT
	'Total number of commercially-held FFELP loans that are 90+ days delinquent?', 
	* 
FROM
(
	SELECT
		count(*) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.bf_ssn,
		ln_seq,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.bf_ssn,
		ln_seq
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP >90
	) P
	CROSS JOIN 
	(
		SELECT
			count(*) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP >90
	) AS CJ

INSERT INTO @DATA	
SELECT
	'Total dollar amount of commercially-held FFELP loans that are 90+ days delinquent?', 
	* 
FROM
(
	SELECT
		SUM(LA_CUR_PRI) AS F
	FROM
	(
	SELECT DISTINCT
		LN16.bf_ssn,
		ln_seq,
		MAX(CASE 
			WHEN LD_DLQ_MAX < '05/15/2020' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
			ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2020')
		END) AS DDP
	FROM
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
	WHERE
		LD_DLQ_OCC < '05/15/2020' --past due before the cutoff date 
		AND LD_DLQ_MAX > '05/15/2020' --past due after
	GROUP BY
		LN16.bf_ssn,
		ln_seq
	) DD
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON LN10.BF_SSN = DD.BF_SSN
		AND LN10.LN_SEQ = DD.LN_SEQ
	WHERE 
		DD.DDP >90
	) P
	CROSS JOIN 
	(
		SELECT
			SUM(LA_CUR_PRI) AS S
		FROM
		(
		SELECT DISTINCT
			LN16.bf_ssn,
			ln_seq,
			MAX(CASE 
				WHEN LD_DLQ_MAX < '05/15/2019' THEN DATEDIFF(DAY, LD_DLQ_OCC, LD_DLQ_MAX)
				ELSE DATEDIFF(DAY, LD_DLQ_OCC, '05/15/2019')
			END) AS DDP
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @BWRS BS
				ON BS.BF_SSN = LN16.BF_SSN
		WHERE
			LD_DLQ_OCC < '05/15/2019' --past due before the cutoff date 
			AND LD_DLQ_MAX > '05/15/2019' --past due after
		GROUP BY
			LN16.bf_ssn,
			ln_seq
		) DD
		INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
			ON LN10.BF_SSN = DD.BF_SSN
			AND LN10.LN_SEQ = DD.LN_SEQ
		WHERE 
			DD.DDP >90
	) AS CJ

INSERT INTO @DATA
SELECT
'Total number of commercially-held FFELP forbearances requested?',
 *
 FROM
 (
		SELECT
			COUNT(*) AS F
		FROM
			UDW..LN10_LON LN10
			INNER JOIN
			(
				SELECT 
					FB10.LC_FOR_TYP,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END,
					FB10.LD_CRT_REQ_FOR,
					FB10.LF_LST_DTS_FB10,
					FB10.LC_FOR_STA,
					LN60.BF_SSN,
					LN60.LN_SEQ
				FROM
					UDW..FB10_BR_FOR_REQ FB10
					INNER JOIN UDW..LN60_BR_FOR_APV LN60
						ON LN60.BF_SSN = FB10.BF_SSN
						AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FB10.BF_SSN
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					--AND FB10.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LN10.BF_SSN
				AND Forb.LN_SEQ = LN10.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN '03/01/2020' AND '05/15/2020'
)P
CROSS JOIN
(
		SELECT
			COUNT(*) AS S
		FROM
			UDW..LN10_LON LN10
			INNER JOIN
			(
				SELECT 
					FB10.LC_FOR_TYP,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END,
					FB10.LD_CRT_REQ_FOR,
					FB10.LF_LST_DTS_FB10,
					FB10.LC_FOR_STA,
					LN60.BF_SSN,
					LN60.LN_SEQ
				FROM
					UDW..FB10_BR_FOR_REQ FB10
					INNER JOIN UDW..LN60_BR_FOR_APV LN60
						ON LN60.BF_SSN = FB10.BF_SSN
						AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FB10.BF_SSN
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					--AND FB10.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LN10.BF_SSN
				AND Forb.LN_SEQ = LN10.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN '03/01/2019' AND '05/15/2019'
) CJ

INSERT INTO @DATA
SELECT
	'Total dollar amount of comercially-held FFELP forbearances granted?',
	*
FROM
(
		SELECT
			SUM(LA_CUR_PRI) AS F
		FROM
			AuditUDW..LN10_LON_Apr2020 LN10
			INNER JOIN
			(
				SELECT 
					FB10.LC_FOR_TYP,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END,
					FB10.LD_CRT_REQ_FOR,
					FB10.LF_LST_DTS_FB10,
					FB10.LC_FOR_STA,
					LN60.BF_SSN,
					LN60.LN_SEQ
				FROM
					UDW..FB10_BR_FOR_REQ FB10
					INNER JOIN UDW..LN60_BR_FOR_APV LN60
						ON LN60.BF_SSN = FB10.BF_SSN
						AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FB10.BF_SSN
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					AND FB10.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LN10.BF_SSN
				AND Forb.LN_SEQ = LN10.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN '03/01/2020' AND '05/15/2020'
) P
CROSS JOIN
(
		SELECT
			SUM(LA_CUR_PRI) AS S
		FROM
			AuditUDW..LN10_LON_Apr2020 LN10
			INNER JOIN
			(
				SELECT 
					FB10.LC_FOR_TYP,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END,
					FB10.LD_CRT_REQ_FOR,
					FB10.LF_LST_DTS_FB10,
					FB10.LC_FOR_STA,
					LN60.BF_SSN,
					LN60.LN_SEQ
				FROM
					UDW..FB10_BR_FOR_REQ FB10
					INNER JOIN UDW..LN60_BR_FOR_APV LN60
						ON LN60.BF_SSN = FB10.BF_SSN
						AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					INNER JOIN @BWRS BS
						ON BS.BF_SSN = FB10.BF_SSN
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					AND FB10.LC_FOR_STA = 'A' 
			) Forb
				ON Forb.BF_SSN = LN10.BF_SSN
				AND Forb.LN_SEQ = LN10.LN_SEQ
WHERE
	FORB.LD_CRT_REQ_FOR BETWEEN '03/01/2019' AND '05/15/2019'
) CJ

INSERT INTO @DATA
SELECT
	'Total number of commercially-held FFELP loans referred to guaranty agencies?',
	*
FROM
(
SELECT
	COUNT(*) AS F
FROM
	AuditUDW..DW01_DW_CLC_CLU_Apr2020 DW01
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DW01.BF_SSN
WHERE
	WC_DW_LON_STA = 14
)P
CROSS JOIN
(
SELECT
	COUNT(*) AS S
FROM
	AuditUDW..DW01_DW_CLC_CLU_Apr2019 DW01
	INNER JOIN AuditUDW..LN10_LON_Apr2019 LN10
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DW01.BF_SSN
WHERE
	WC_DW_LON_STA = 14
) CJ

INSERT INTO @DATA
SELECT
	'Total dollar amount of commercially-held FFELP loans referred to guaranty agencies?',
	*
FROM
(
SELECT
	SUM(LA_CUR_PRI) AS F
FROM
	AuditUDW..DW01_DW_CLC_CLU_Apr2020 DW01
	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DW01.BF_SSN
WHERE
	WC_DW_LON_STA = 14
)P
CROSS JOIN
(
SELECT
	SUM(LA_CUR_PRI) AS S
FROM
	AuditUDW..DW01_DW_CLC_CLU_Apr2019 DW01
	INNER JOIN AuditUDW..LN10_LON_Apr2019 LN10
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN @BWRS BS
		ON BS.BF_SSN = DW01.BF_SSN
WHERE
	WC_DW_LON_STA = 14
) CJ

INSERT INTO @DATA VALUES
('Total Number of Perkins loans being serviced?',' ',' '),
('Total dollar amount of Perkins loans being serviced?',' ',' '),
('Total number of Perkins loans that are 30 days delinquent?',' ',' '),
('Total dollar amount of Perkins loans that are 30 days delinquent?',' ',' '),
('Total number of Perkins loans that are 60 days delinquent?',' ',' '),
('Total dollar amount of Perkins loans that are 60 days delinquent?',' ',' '),
('Total number of Perkins loans that are 90+ days delinquent?',' ',' '),
('Total dollar amount of Perkins loans that are 90+ days delinquent?',' ',' '),
('Total number of Perkins forbearances requested?',' ',' '),
('Total dollar amount of Perkins forbearances granted?',' ',' '),
('Total number of Perkins loans referred to third-party debt collectors ?',' ',' '),
('Total dollar amount of Perkins loans referred to third-party debt collectors?',' ',' '),
('Total Number of Private loans being serviced?',' ',' '),
('Total dollar amount of Private loans being serviced?',' ',' '),
('Total number of Private loans that are 30 days delinquent?',' ',' '),
('Total dollar amount of Private loans that are 30 days delinquent?',' ',' '),
('Total number of Private loans that are 60 days delinquent?',' ',' '),
('Total dollar amount of Private loans that are 60 days delinquent?',' ',' '),
('Total number of Private loans that are 90+ days delinquent?',' ',' '),
('Total dollar amount of Private loans that are 90+ days delinquent?',' ',' '),
('Total number of Private forbearances requested?',' ',' '),
('Total dollar amount of Private forbearances granted?',' ',' '),
('Total number of Private loans referred to third-party debt collectors ?',' ',' '),
('Total dollar amount of Private loans referred to third-party debt collectors?',' ',' ')



SELECT * FROM @DATA