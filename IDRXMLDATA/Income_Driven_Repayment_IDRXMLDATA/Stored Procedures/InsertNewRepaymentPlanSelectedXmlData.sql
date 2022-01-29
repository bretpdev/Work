CREATE PROCEDURE [idrxmldata].[InsertNewRepaymentPlanSelectedXmlData]
	@ApplicationId INT,
	@RepaymentTypeId INT
AS

INSERT INTO [dbo].[Repayment_Plan_Selected] (application_id, repayment_type_id)
VALUES(@ApplicationId, @RepaymentTypeId)
	
SELECT CAST(SCOPE_IDENTITY() AS INT)
