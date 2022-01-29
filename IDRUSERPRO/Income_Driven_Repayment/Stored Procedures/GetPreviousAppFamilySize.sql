
CREATE PROCEDURE [dbo].[GetPreviousAppFamilySize]
	@SSN CHAR(9),
	@CurrentAppId INT = 0
AS
	SELECT
		APP.family_size
	FROM 
		Applications APP
		INNER JOIN
		(
			SELECT DISTINCT
				MAX(APP.application_id) AS app_id
			FROM
				Borrowers BOR
				INNER JOIN Loans LOAN
					ON BOR.borrower_id = LOAN.borrower_id
				INNER JOIN Applications APP
					ON LOAN.application_id = APP.application_id
				INNER JOIN Repayment_Plan_Selected RPS
					on RPS.application_id = APP.application_id
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
				(@SSN = BOR.SSN)
				AND STA.repayment_plan_type_status_id = 1
				AND APP.application_id != @CurrentAppId
		) MAX_VAL
			ON MAX_VAL.app_id = APP.application_id

RETURN 0