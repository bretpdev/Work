-- =============================================
-- Author:		Daren Beattie
-- Create date: November 18, 2011
-- =============================================
CREATE PROCEDURE [dbo].[AddEBillRecord]
	-- Add the parameters for the stored procedure here
	@SSN CHAR(9),
	@LoanSequence INT,
	@BillingPreference CHAR(1),
	@Email varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO EBill (
		SSN,
		LoanSequence,
		BillingPreference,
		EmailAddress
	)
	VALUES (
		@SSN,
		@LoanSequence,
		@BillingPreference,
		@Email
	)

	SELECT SCOPE_IDENTITY()
END


