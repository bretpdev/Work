-- =============================================
-- Author:		JAROM RYAN 
-- Create date: 06/03/2013
-- Description:	THIS WILL PULL THE STATUS AND SUBSTATUS FOR AN EXISTING APPLICATION. 
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingAppStatus] 

@AppId INT

AS
BEGIN

	SELECT 
		STA.repayment_plan_type_status + ',' + SUB.repayment_plan_type_substatus
	FROM
		Repayment_Plan_Selected RPS
	INNER JOIN Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN dbo.Repayment_Plan_Type_Substatus SUB
		ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN dbo.Repayment_Plan_Type_Status STA
		ON STA.repayment_plan_type_status_id = SUB.repayment_plan_type_status_id
	WHERE 
		RPS.application_id = @AppId
	
	
END