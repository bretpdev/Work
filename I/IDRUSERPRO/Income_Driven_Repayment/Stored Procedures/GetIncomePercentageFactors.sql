CREATE PROCEDURE [dbo].[GetIncomePercentageFactors]
	@CurrentDate DATETIME = NULL 
AS

	IF (@CurrentDate IS NULL)
		SET @CurrentDate = GETDATE();

	SELECT
		income, factor, 
		[start_date], 
		end_date, 
		married_or_head_of_household
	FROM
		dbo.Income_Percentage_Factors
	WHERE
		@CurrentDate BETWEEN [start_date] AND [end_date]

RETURN 0
