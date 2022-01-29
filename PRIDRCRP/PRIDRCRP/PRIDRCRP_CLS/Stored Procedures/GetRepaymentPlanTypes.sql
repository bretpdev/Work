CREATE PROCEDURE [pridrcrp].[GetRepaymentPlanTypes]
AS
	
SELECT
	[RepaymentPlanTypeId],
	[RepaymentPlanType]
FROM
	[pridrcrp].[RepaymentPlanTypes]
