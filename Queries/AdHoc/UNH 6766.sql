----What is the number of phone calls received during the time frame for Private loans, commercially-held loans, and Perkins loans?
--SELECT 
--	COUNT(*)
--FROM
--	NobleCalls..NobleCallHistory
--WHERE
--	RegionId = 2
--	AND DeletedAt IS NULL
--	AND CreatedAt BETWEEN '03/01/2020' AND '05/15/2020'
--	AND IsInbound = 1

--SELECT 
--	COUNT(*)
--FROM
--	NobleCalls..NobleCallHistory
--WHERE
--	RegionId = 2
--	AND DeletedAt IS NULL
--	AND CreatedAt BETWEEN '03/01/2019' AND '05/15/2019'
--	AND IsInbound = 1

--SELECT
--	COUNT(*)
--FROM
--	IncomeBasedRepaymentUheaa..Applications
--WHERE
--	ACTIVE = 1
--	AND created_at BETWEEN '03/01/2020' AND '05/15/2020'


--SELECT
--	COUNT(*)
--FROM
--	IncomeBasedRepaymentUheaa..Applications
--WHERE
--	ACTIVE = 1
--	AND created_at BETWEEN '03/01/2019' AND '05/15/2019'

	--SELECT
	--	COUNT(DISTINCT APP.application_id)

	--FROM
	--	IncomeBasedRepaymentUheaa..Applications App
	--	inner join IncomeBasedRepaymentUheaa..Repayment_Plan_Selected RPS
	--		on RPS.application_id = App.application_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History HIS
	--		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Substatus SUB
	--		ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status STA
	--		ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
	--	inner join
	--	(
	--		select	
	--			h.repayment_plan_type_id,
	--			max(h.created_at) as created_at

	--		from
	--			IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History h
	--		group by
	--			h.repayment_plan_type_id
	--	) max_date
	--		on his.repayment_plan_type_id = max_date.repayment_plan_type_id
	--		and his.created_at = max_date.created_at

	--WHERE
	--	APP.created_at  BETWEEN '03/01/2020' AND '05/15/2020'
	--	AND Active = 1
	--	AND sta.repayment_plan_type_status = 'APPROVED'


	--SELECT
	--	COUNT(DISTINCT APP.application_id)

	--FROM
	--	IncomeBasedRepaymentUheaa..Applications App
	--	inner join IncomeBasedRepaymentUheaa..Repayment_Plan_Selected RPS
	--		on RPS.application_id = App.application_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History HIS
	--		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Substatus SUB
	--		ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	--	INNER JOIN IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status STA
	--		ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
	--	inner join
	--	(
	--		select	
	--			h.repayment_plan_type_id,
	--			max(h.created_at) as created_at

	--		from
	--			IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Status_History h
	--		group by
	--			h.repayment_plan_type_id
	--	) max_date
	--		on his.repayment_plan_type_id = max_date.repayment_plan_type_id
	--		and his.created_at = max_date.created_at

	--WHERE
	--	APP.created_at  BETWEEN '03/01/2019' AND '05/15/2019'
	--	AND Active = 1
	--	AND sta.repayment_plan_type_status = 'APPROVED'

--SELECT
--	COUNT(*),
--	SUM(LA_CUR_PRI)
--FROM
--	AuditUDW..LN10_LON_Apr2020
--WHERE
--	LA_CUR_PRI > 0
--	AND LC_STA_LON10 = 'R'

--SELECT
--	COUNT(*),
--	SUM(LA_CUR_PRI)
--FROM
--	AuditUDW..LN10_LON_Apr2019
--WHERE
--	LA_CUR_PRI > 0
--	AND LC_STA_LON10 = 'R'

--SELECT
--	COUNT(*),
--	SUM(LN10.LA_CUR_PRI)
--FROM
--	UDW.CALC.DailyDelinquency DD
--	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
--		ON LN10.BF_SSN = DD.BF_SSN
--		AND LN10.LN_SEQ = DD.LN_SEQ
--WHERE
--	AddedAt = '05/15/2020'
--	AND DD.LN_DLQ_MAX BETWEEN 1 AND 30

--SELECT
--	COUNT(*),
--	SUM(LN10.LA_CUR_PRI)
--FROM
--	UDW.CALC.DailyDelinquency DD
--	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
--		ON LN10.BF_SSN = DD.BF_SSN
--		AND LN10.LN_SEQ = DD.LN_SEQ
--WHERE
--	AddedAt = '05/15/2020'
--	AND DD.LN_DLQ_MAX BETWEEN 31 AND 89

--SELECT
--	COUNT(*),
--	SUM(LN10.LA_CUR_PRI)
--FROM
--	UDW.CALC.DailyDelinquency DD
--	INNER JOIN AuditUDW..LN10_LON_Apr2020 LN10
--		ON LN10.BF_SSN = DD.BF_SSN
--		AND LN10.LN_SEQ = DD.LN_SEQ
--WHERE
--	AddedAt = '05/15/2020'
--	AND DD.LN_DLQ_MAX  >= 90
