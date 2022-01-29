-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/22/2013
-- Description:	Will get all data from dbo.Repayment_Plan_Type_Status
-- =============================================
CREATE PROCEDURE [dbo].[spGetPlanStatuses]
	
AS
BEGIN

	SELECT 
		repayment_plan_type_status_id AS StatusId,
		repayment_plan_type_status As Status
	FROM 
		dbo.Repayment_Plan_Type_Status
END