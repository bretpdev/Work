-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/17/2013
-- Description:	Will get the Repayment Plan Reasons
-- =============================================
CREATE PROCEDURE [dbo].[spGetRepaymentPlanReason] 
AS
BEGIN

	SELECT 
		repayment_plan_reason_id AS RepaymentPlanReasonId,
		repayment_plan_reason_code AS RepaymentPlanReasonCode,
		repayment_plan_reason_description AS RepaymentPlanReasonDescription
	FROM dbo.Repayment_Plan_Reason
	
END