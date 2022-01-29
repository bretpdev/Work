CREATE PROCEDURE [dbo].[GetIdentifiedAccounts]
	@SSN char(9) = ''
AS
	SELECT DISTINCT
		@SSN AS [Account_Identifier],
		APP.received_date AS Received_Date,
		SOURCE.application_source [Application_Type],
		APP.e_application_id AS [E_App_ID],
		APP.application_id AS App_ID,
		STA.repayment_plan_type_status AS [Status]
	FROM
		Borrowers BOR
		INNER JOIN Loans LOAN
			ON BOR.borrower_id = LOAN.borrower_id
		INNER JOIN Applications APP
			ON LOAN.application_id = APP.application_id
		INNER JOIN Application_Source SOURCE
			ON APP.application_source_id = SOURCE.application_source_id
		INNER JOIN Repayment_Plan_Selected RPS
			ON RPS.application_id = APP.application_id
		INNER JOIN Repayment_Plan_Type_Status_History HIS
			ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		INNER JOIN dbo.Repayment_Plan_Type_Substatus SUB
			ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
		INNER JOIN dbo.Repayment_Plan_Type_Status STA
			ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
		INNER JOIN
		(
			SELECT	
				H.repayment_plan_type_id,
				MAX(H.created_at) AS created_at
			FROM
				Repayment_Plan_Type_Status_History H
			GROUP BY
				H.repayment_plan_type_id
		) MAX_DATE
			ON HIS.repayment_plan_type_id = MAX_DATE.repayment_plan_type_id
			AND HIS.created_at = MAX_DATE.created_at
	WHERE
		(@SSN = Bor.SSN)
	ORDER BY 
		APP.application_id

RETURN 0