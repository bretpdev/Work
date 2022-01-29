
	
--complete	
	 SELECT DISTINCT
		sum(CASE 
			WHEN App.created_at between '02/24/2020' and '03/08/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d1,
		sum(CASE 
			WHEN App.created_at between '03/09/2020' and '03/22/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d2,
		sum(CASE 
			WHEN App.created_at between '03/03/2020' and '04/05/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d3,
		sum(CASE 
			WHEN App.created_at between '04/05/2020' and '04/19/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d4,
		sum(CASE 
			WHEN App.created_at between '04/20/2020' and '05/03/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END 
		end) over() AS d5,
		sum(CASE 
			WHEN App.created_at between '05/04/2020' and '05/17/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d6,
		sum(CASE 
			WHEN App.created_at between '05/17/2020' and '05/31/2020' then 
			case
				WHEN sta.repayment_plan_type_status IN ('APPROVED','DENIED') THEN 1
				ELSE 0
			END 
		end) over() AS d7
		-- SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('PENDING') THEN 1
		--	ELSE 0
		--END) AS INCOMPLETE,
		--SUM(CASE WHEN sta.repayment_plan_type_status IN ('APPROVED') THEN 1
		--	ELSE 0
		--END) AS APPROVED,
		--SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS DENIED
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
		ON l.application_id = App.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN 
	(
		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD30_PRS_ADR PD30
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD30.DC_ADR = 'L'
			AND PD30.DC_DOM_ST = 'NY'
			AND CAST(PD30.DF_LST_DTS_PD30 AS DATE) <= CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD31_PRS_INA PD31
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD31.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD31.DC_ADR_HST = 'L'
			AND PD31.DC_DOM_ST_HST = 'NY'
			AND CAST(PD31.DD_CRT_PD31 AS DATE) BETWEEN CAST('2019-10-09' AS DATE) AND CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
	) ny
		on ny.bf_ssn = b.ssn

	WHERE
		APP.created_at  BETWEEN '02/24/2020' AND '05/31/2020'
		AND Active = 1

--incomplete
SELECT DISTINCT
		sum(CASE 
			WHEN App.created_at between '02/24/2020' and '03/08/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END
		end) over() AS d1,
		sum(CASE 
			WHEN App.created_at between '03/09/2020' and '03/22/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END
		end) over() AS d2,
		sum(CASE 
			WHEN App.created_at between '03/03/2020' and '04/05/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END
		end) over() AS d3,
		sum(CASE 
			WHEN App.created_at between '04/05/2020' and '04/19/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END
		end) over() AS d4,
		sum(CASE 
			WHEN App.created_at between '04/20/2020' and '05/03/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END 
		end) over() AS d5,
		sum(CASE 
			WHEN App.created_at between '05/04/2020' and '05/17/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END
		end) over() AS d6,
		sum(CASE 
			WHEN App.created_at between '05/17/2020' and '05/31/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('PENDING') THEN 1
				ELSE 0
			END 
		end) over() AS d7
		-- SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('PENDING') THEN 1
		--	ELSE 0
		--END) AS INCOMPLETE,
		--SUM(CASE WHEN sta.repayment_plan_type_status IN ('APPROVED') THEN 1
		--	ELSE 0
		--END) AS APPROVED,
		--SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS DENIED
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
		ON l.application_id = App.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN 
	(
		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD30_PRS_ADR PD30
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD30.DC_ADR = 'L'
			AND PD30.DC_DOM_ST = 'NY'
			AND CAST(PD30.DF_LST_DTS_PD30 AS DATE) <= CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD31_PRS_INA PD31
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD31.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD31.DC_ADR_HST = 'L'
			AND PD31.DC_DOM_ST_HST = 'NY'
			AND CAST(PD31.DD_CRT_PD31 AS DATE) BETWEEN CAST('2019-10-09' AS DATE) AND CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
	) ny
		on ny.bf_ssn = b.ssn

	WHERE
		APP.created_at  BETWEEN '02/24/2020' AND '05/31/2020'
		AND Active = 1

--APPROVED
SELECT DISTINCT
		sum(CASE 
			WHEN App.created_at between '02/24/2020' and '03/08/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END
		end) over() AS d1,
		sum(CASE 
			WHEN App.created_at between '03/09/2020' and '03/22/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END
		end) over() AS d2,
		sum(CASE 
			WHEN App.created_at between '03/03/2020' and '04/05/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END
		end) over() AS d3,
		sum(CASE 
			WHEN App.created_at between '04/05/2020' and '04/19/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END
		end) over() AS d4,
		sum(CASE 
			WHEN App.created_at between '04/20/2020' and '05/03/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END 
		end) over() AS d5,
		sum(CASE 
			WHEN App.created_at between '05/04/2020' and '05/17/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END
		end) over() AS d6,
		sum(CASE 
			WHEN App.created_at between '05/17/2020' and '05/31/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('APPROVED') THEN 1
				ELSE 0
			END 
		end) over() AS d7
		-- SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('APPROVED') THEN 1
		--	ELSE 0
		--END) AS INCOMPLETE,
		--SUM(CASE WHEN sta.repayment_plan_type_status IN ('APPROVED') THEN 1
		--	ELSE 0
		--END) AS APPROVED,
		--SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS DENIED
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
		ON l.application_id = App.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN 
	(
		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD30_PRS_ADR PD30
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD30.DC_ADR = 'L'
			AND PD30.DC_DOM_ST = 'NY'
			AND CAST(PD30.DF_LST_DTS_PD30 AS DATE) <= CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD31_PRS_INA PD31
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD31.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD31.DC_ADR_HST = 'L'
			AND PD31.DC_DOM_ST_HST = 'NY'
			AND CAST(PD31.DD_CRT_PD31 AS DATE) BETWEEN CAST('2019-10-09' AS DATE) AND CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
	) ny
		on ny.bf_ssn = b.ssn

	WHERE
		APP.created_at  BETWEEN '02/24/2020' AND '05/31/2020'
		AND Active = 1


SELECT DISTINCT
		sum(CASE 
			WHEN App.created_at between '02/24/2020' and '03/08/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d1,
		sum(CASE 
			WHEN App.created_at between '03/09/2020' and '03/22/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d2,
		sum(CASE 
			WHEN App.created_at between '03/03/2020' and '04/05/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d3,
		sum(CASE 
			WHEN App.created_at between '04/05/2020' and '04/19/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d4,
		sum(CASE 
			WHEN App.created_at between '04/20/2020' and '05/03/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END 
		end) over() AS d5,
		sum(CASE 
			WHEN App.created_at between '05/04/2020' and '05/17/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END
		end) over() AS d6,
		sum(CASE 
			WHEN App.created_at between '05/17/2020' and '05/31/2020' then 
			case
				WHEN  sta.repayment_plan_type_status IN ('DENIED') THEN 1
				ELSE 0
			END 
		end) over() AS d7
<<<<<<< HEAD
		
=======
		-- SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS INCOMPLETE,
		--SUM(CASE WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS APPROVED,
		--SUM(CASE 
		--	WHEN sta.repayment_plan_type_status IN ('DENIED') THEN 1
		--	ELSE 0
		--END) AS DENIED
>>>>>>> 6111bdab7014e5ddcce25ddf06792b96cc29a57e
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
		ON l.application_id = App.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN 
	(
		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD30_PRS_ADR PD30
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD30.DC_ADR = 'L'
			AND PD30.DC_DOM_ST = 'NY'
			AND CAST(PD30.DF_LST_DTS_PD30 AS DATE) <= CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		SELECT DISTINCT
			LN10.BF_SSN
		FROM
			UDW..PD31_PRS_INA PD31
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD31.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN AuditUDW..LN10_LON_May2020 LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD31.DC_ADR_HST = 'L'
			AND PD31.DC_DOM_ST_HST = 'NY'
			AND CAST(PD31.DD_CRT_PD31 AS DATE) BETWEEN CAST('2019-10-09' AS DATE) AND CAST('2020-05-31' AS DATE)
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
	) ny
		on ny.bf_ssn = b.ssn

	WHERE
		APP.created_at  BETWEEN '02/24/2020' AND '05/31/2020'
		AND Active = 1
		
