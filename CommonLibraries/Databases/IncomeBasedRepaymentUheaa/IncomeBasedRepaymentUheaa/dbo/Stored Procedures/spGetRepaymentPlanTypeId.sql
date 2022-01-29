-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/05/2013
-- Description:	WILL GET THE REPAYMENT PLAN TYPE FOR A GIVEN APP ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetRepaymentPlanTypeId]

@AppId INT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		repayment_plan_type_id
	FROM 
		dbo.Repayment_Plan_Selected
	WHERE 
		application_id = @AppId
END
