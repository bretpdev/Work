-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/05/2013
-- Description:	WILL GET THE REPAYMENT PLAN TYPE FOR A GIVEN APP ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetRepaymentPlanTypeId]
	@AppId INT
AS
BEGIN

	SELECT 
		repayment_plan_type_id [RepaymentPlanTypeId],
		repayment_type_id [RepaymentTypeId]
	FROM 
		dbo.Repayment_Plan_Selected
	WHERE 
		application_id = @AppId
END