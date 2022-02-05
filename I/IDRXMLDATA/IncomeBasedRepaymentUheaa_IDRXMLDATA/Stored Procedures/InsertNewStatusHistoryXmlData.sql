CREATE PROCEDURE [idrxmldata].[InsertNewStatusHistoryXmlData]
	@RepaymentPlanTypeId INT
AS

INSERT INTO [Repayment_Plan_Type_Status_History] (repayment_plan_type_id, repayment_plan_type_status_mapping_id, created_at, created_by)
SELECT
	@RepaymentPlanTypeId,
	repayment_plan_type_substatus_id,
	GETDATE(),
	'IDRXMLDATA'
FROM
	IncomeBasedRepaymentUheaa..Repayment_Plan_Type_Substatus
WHERE
	repayment_plan_type_substatus ='Application Pending - Other'