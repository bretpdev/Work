-- =============================================
-- Author:		<Author,,Name>
-- Create date: 05/24/2013
-- Description:	will get data for a given status and reason id
-- =============================================
CREATE PROCEDURE [dbo].[spGetSubStatuses] 

	@Status int,
	@ReasonId int
	
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		repayment_plan_type_substatus_id AS SubStatusId,
		repayment_plan_type_status_id AS TypeId,
		repayment_type_status_id AS StatusCodeId,
		repayment_plan_reason_id AS ReasonId,
		repayment_plan_type_substatus AS SubStatus
	FROM 
		dbo.Repayment_Plan_Type_Substatus
	WHERE
		repayment_plan_type_status_id = @Status
		AND repayment_plan_reason_id = @ReasonId
END
