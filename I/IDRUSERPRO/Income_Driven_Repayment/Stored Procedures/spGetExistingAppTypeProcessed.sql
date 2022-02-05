CREATE PROCEDURE [dbo].[spGetExistingAppTypeProcessed]
	@AppId INT
AS
BEGIN
	SELECT 
		RPT.repayment_plan
	FROM
		dbo.Repayment_Plan_Selected RPS
		INNER JOIN dbo.Repayment_Type RT
			ON RT.repayment_type_id = RPS.repayment_type_id
		INNER JOIN dbo.Repayment_Plan_Type RPT
			ON RPT.repayment_plan_type_id = RT.repayment_plan_type_id
		INNER JOIN Applications A
			ON RPS.application_id = A.application_id
	WHERE 
		RPS.application_id = @AppId
		AND A.updated_by != 'IDRXMLDATA'
END