-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/30/2013
-- Description:	WILL INSERT DATA INTO THE dbo.Repayment_Plan_Type_Status_History TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spInsertStatusHistory] 

@RepaymentPlanTypeId int,
@RepaymentPlanStatusMappingId int,
@CreatedBy Varchar(50)


AS
BEGIN

	SET NOCOUNT ON;

INSERT INTO dbo.Repayment_Plan_Type_Status_History(repayment_plan_type_id,repayment_plan_type_status_mapping_id,created_by)
VALUES (@RepaymentPlanTypeId, @RepaymentPlanStatusMappingId, @CreatedBy)


END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spInsertStatusHistory] TO [db_executor]
    AS [dbo];

