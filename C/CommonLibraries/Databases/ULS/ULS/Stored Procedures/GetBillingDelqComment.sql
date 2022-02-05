CREATE PROCEDURE [dbo].[GetBillingDelqComment]
	@DaysDelinquent int = 0
AS

	SELECT
		Comment
	FROM
		BillingDelinquentComment
	WHERE
		@DaysDelinquent >= StartCount
		AND @DaysDelinquent <= EndCount
		 
RETURN 0
