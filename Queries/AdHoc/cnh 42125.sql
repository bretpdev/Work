USE [Income_Driven_Repayment]
GO
/****** Object:  StoredProcedure [dbo].[spGetArcAndComment]    Script Date: 6/5/2020 8:38:11 AM ******/


	SELECT distinct
		b.account_number,
		HIS.repayment_plan_type_status_history_id AS HisId,
		HIS.repayment_plan_type_id,
		SUBSTA.*,
		a.*
	FROM 
		[Income_Driven_Repayment]..Repayment_Plan_Selected RPS
	inner join Income_Driven_Repayment..Applications A
		ON A.application_id = Rps.application_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Plan_Type_Substatus SUBSTA
		ON SUBSTA.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Type_Status RTS
		ON RTS.repayment_type_status_id = SUBSTA.repayment_type_status_id
	inner join Income_Driven_Repayment..Loans l
		on l.application_id = a.application_id
	inner join Income_Driven_Repayment..Borrowers b
		on b.borrower_id = l.borrower_id
			inner join
		(
			select	
				h.repayment_plan_type_id,
				max(h.created_at) as created_at

			from
				Repayment_Plan_Type_Status_History h
			group by
				h.repayment_plan_type_id
		) max_date
			on his.repayment_plan_type_id = max_date.repayment_plan_type_id
			and his.created_at = max_date.created_at
	left join cls..ArcAddProcessing aap
		on aap.AccountNumber = b.account_number
		and aap.ARC = 'IDRPN'
		and aap.CreatedAt > '03/03/2020'
	WHERE a.created_at > '03/03/2020'
	AND SUBSTA.repayment_plan_type_status_id = 6
	and aap.AccountNumber is null
	and updated_by != 'IDRXMLDATA'
order by b.account_number