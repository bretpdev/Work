-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/29/2013
-- Description:	THIS WILL ENTER DATA INTO THE LOANS TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spInsertLoanData]
	
@BorrowerId INT,
@AppId INT,
@LoanType CHAR(6) = NULL,
@AwardId CHAR(21),
@LoanSeq Char(4)= NULL,
@DisbDate VARCHAR(8) = NULL	
	
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.Loans(borrower_id,application_id,loan_type,award_id, loan_seq, disb_date)
	VALUES(@BorrowerId,@AppId,@LoanType,@AwardId,@LoanSeq, @DisbDate)
	
END
