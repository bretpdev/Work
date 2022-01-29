CREATE PROCEDURE [dbo].[InsertNewRepaymentPlanSelectedXmlData]
	@ApplicationId CHAR(10) = NULL,
	@RepaymentTypeId INT
AS

INSERT INTO [Income_Driven_Repayment].[dbo].[Repayment_Plan_Selected] (application_id, repayment_type_id)
VALUES(@ApplicationId,@RepaymentTypeId)
	
SELECT CAST(SCOPE_IDENTITY() AS INT)