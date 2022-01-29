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

	SET NOCOUNT ON;

	UPDATE dbo.Repayment_Plan_Selected
	SET repayment_type_id = @newRepayId
	WHERE repayment_plan_type_id = @repayPlanTypeId


END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateSelectedPlan] TO [db_executor]
    AS [dbo];

