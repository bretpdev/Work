-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/24/2013
-- Description:	Will get all of the data from dbo.Repayment_Plan_Type
-- =============================================
CREATE PROCEDURE [dbo].[spGetPlanTypes]



AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		repayment_plan_type_id AS PlanId,
		repayment_plan AS [Plan]
	FROM 
		dbo.Repayment_Plan_Type
END
