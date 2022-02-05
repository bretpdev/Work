
CREATE PROCEDURE [dbo].[GetRequestedPlans]
	@AppId INT
AS
	SELECT 
		RT.repayment_type_code [ProcessedRepaymentPlan],
		REQ.repayment_plan_type_requested_description [RequestedRepaymentPlans],
		App.borrower_selected_lowest_plan [LowestPlanRequested]
	FROM
		dbo.Applications APP
		INNER JOIN dbo.Repayment_Plan_Type_Requested REQ
			ON REQ.repayment_plan_type_requested_id = APP.repayment_plan_type_requested_id
		LEFT JOIN dbo.Repayment_Plan_Selected RPS
			ON RPS.application_id = APP.application_id
		LEFT JOIN dbo.Repayment_Type RT
			ON RT.repayment_type_id = RPS.repayment_type_id
	WHERE
		APP.application_id = @AppId
RETURN 0
