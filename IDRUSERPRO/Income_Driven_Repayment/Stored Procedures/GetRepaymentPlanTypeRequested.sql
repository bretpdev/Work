CREATE PROCEDURE [dbo].[GetRepaymentPlanTypeRequested]
AS
BEGIN
	SELECT
		repayment_plan_type_requested_id AS RepaymentPlanTypeRequestedId,
		repayment_plan_type_requested_description AS RepaymentPlanTypeRequestedDescription
	FROM
		Repayment_Plan_Type_Requested
END