SELECT DISTINCT
	B.first_name AS FIRST_NAME, 
	B.last_name AS LAST_NAME,
	B.SSN,
	RPTSH.created_at AS PROCESSED_DATE,
	CASE
		WHEN RPS.repayment_type_id IN (X,X) THEN 'IBR' --REPAYMENT TYPE TABLE TRACKS IBR A LITTLE DIFFERENT SO THERE ARE X CASES WHERE IT CAN BE IBR
		WHEN RPS.repayment_type_id = X THEN 'ICR' 
		WHEN RPS.repayment_type_id = X THEN 'PAYE'
		WHEN RPS.repayment_type_id = X THEN 'REPAYE'
		ELSE RT.repayment_type_description --FALL OUT SHOULD ANYTHING ADDED IN THE FUTURE
	END AS [PLAN],
	RPTS.repayment_plan_type_status AS [STATUS],
	RPR.repayment_plan_reason_description AS [TYPE]
FROM
	Income_Driven_Repayment..Applications App
	INNER JOIN Income_Driven_Repayment.dbo.Repayment_Plan_Selected RPS
		ON RPS.application_id = APP.application_id
	INNER JOIN --GET THE MOST RECENT HISTORY RECORD FOR AN APP
	(
		SELECT 
			HIS.*
		FROM
			Income_Driven_Repayment..Repayment_Plan_Type_Status_History HIS
			INNER JOIN 
			(
				SELECT
					MAX(R.repayment_plan_type_status_history_id) AS repayment_plan_type_status_history_id, --get the most recent history record
					R.repayment_plan_type_id
				FROM
					Income_Driven_Repayment..Repayment_Plan_Type_Status_History R
				GROUP BY
					r.repayment_plan_type_id
			) MAX_REC
				ON MAX_REC.repayment_plan_type_status_history_id = HIS.repayment_plan_type_status_history_id
	)RPTSH
		ON RPTSH.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Substatus SUB
		ON SUB.repayment_plan_type_substatus_id = RPTSH.repayment_plan_type_status_mapping_id
	INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Status RPTS
		ON RPTS.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
	INNER JOIN Income_Driven_Repayment..Loans L
		ON L.application_id = APP.application_id
	INNER JOIN Income_Driven_Repayment..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type RPT
		ON RPT.repayment_plan_type_id = APP.repayment_plan_reason_id
	INNER JOIN Income_Driven_Repayment.dbo.Repayment_Plan_Reason RPR
		ON RPR.repayment_plan_reason_id = APP.repayment_plan_reason_id
	INNER JOIN Income_Driven_Repayment..Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
WHERE
	RPTSH.created_at BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND SUB.repayment_plan_type_status_id IN (X,X) 
ORDER BY
	B.SSN
	 