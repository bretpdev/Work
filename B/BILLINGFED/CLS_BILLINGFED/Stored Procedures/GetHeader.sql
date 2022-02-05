CREATE PROCEDURE billing.[GetHeader]

AS
	SELECT 
		BillingHeader
	FROM	
		billing.BillingHeaders
	WHERE
		InactivatedAt IS NULL
RETURN 0