
SELECT DISTINCT 
	BorrowerSSN, 
	AwardID, 
	AwardIDSequence
FROM 
	[EAXX].[dbo].[_XXPaymentDataRecord]
WHERE 
	RepaymentPlanCode in ('CX','CX','CX','IB','IL') 