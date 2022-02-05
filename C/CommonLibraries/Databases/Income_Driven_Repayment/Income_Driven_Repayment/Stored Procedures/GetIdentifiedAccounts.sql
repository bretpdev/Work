CREATE PROCEDURE [dbo].[GetIdentifiedAccounts]
	@SSN char(9) = ''
AS
	SELECT DISTINCT
		@SSN AS [Account_Identifier],
		App.received_date AS Received_Date,
		CASE
			WHEN App.e_application_id = '' THEN 'Paper'
			ELSE 'Electronic'
		END AS [Application_Type],
		App.e_application_id AS [E_App_ID],
		App.application_id AS App_ID,
		sta.repayment_plan_type_status as [Status]

	FROM
		Borrowers Bor
		INNER JOIN Loans Loan
			ON Bor.borrower_id = Loan.borrower_id
		INNER JOIN Applications App
			ON loan.application_id = App.application_id
		inner join Repayment_Plan_Selected RPS
			on RPS.application_id = App.application_id
		INNER JOIN Repayment_Plan_Type_Status_History HIS
			ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		INNER JOIN dbo.Repayment_Plan_Type_Substatus SUB
			ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
		INNER JOIN dbo.Repayment_Plan_Type_Status STA
			ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
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

	WHERE
		(@SSN = Bor.SSN)
	ORDER BY 
		App.application_id
RETURN 0

GRANT EXECUTE ON GetIdentifiedAccounts TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIdentifiedAccounts] TO [db_executor]
    AS [dbo];

