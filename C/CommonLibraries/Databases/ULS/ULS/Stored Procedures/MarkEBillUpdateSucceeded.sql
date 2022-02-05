-- =============================================
-- Author:		Daren Beattie
-- Create date: November 19, 2011
-- =============================================
CREATE PROCEDURE MarkEBillUpdateSucceeded
	-- Add the parameters for the stored procedure here
	@EbillId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE 
		EBill
	SET 
		UpdateSucceeded = 1,
		UpdatedAt = GETDATE()
	WHERE 
		EbillId = @EbillId
END
