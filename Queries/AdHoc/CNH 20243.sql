use Income_Driven_Repayment
go

SELECT DISTINCT
	B.account_number,
	ISNULL(APP.repayment_plan_type_requested_id, X) AS repayment_plan_type_requested_id,
	APP.created_at
FROM 
	Repayment_Plan_Type_Status_History RPTSH 
	INNER JOIN [Repayment_Plan_Selected] RPS
		ON RPS.repayment_plan_type_id = RPTSH.repayment_plan_type_id
	INNER JOIN Applications APP
		ON APP.application_id = RPS.application_id
	INNER JOIN Loans L
		ON L.application_id = RPS.application_id
	INNER JOIN Borrowers B
		ON B.borrower_id = L.borrower_id
WHERE 
	RPTSH.repayment_plan_type_status_mapping_id = XX 
	AND APP.repayment_plan_status_id = X