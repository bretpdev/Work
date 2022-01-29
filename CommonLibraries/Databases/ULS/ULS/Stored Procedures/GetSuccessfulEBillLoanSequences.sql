-- =============================================
-- Author:		Daren Beattie
-- Create date: November 19, 2011
-- =============================================
CREATE PROCEDURE [dbo].[GetSuccessfulEBillLoanSequences]
	-- Add the parameters for the stored procedure here
	@SSN CHAR(9),
	@BillingPreference CHAR(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		LoanSequence
	FROM 
		EBill
	WHERE 
		SSN = @SSN
		AND BillingPreference = @BillingPreference
		AND UpdateSucceeded = 1
		AND ArcAdded = 0
		AND HadError = 0
END

