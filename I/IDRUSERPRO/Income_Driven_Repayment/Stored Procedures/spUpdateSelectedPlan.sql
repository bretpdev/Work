-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateSelectedPlan]
	@repayPlanTypeId INT,
	@newRepayId INT
AS
BEGIN

	UPDATE dbo.Repayment_Plan_Selected
	SET repayment_type_id = @newRepayId
	WHERE repayment_plan_type_id = @repayPlanTypeId


END