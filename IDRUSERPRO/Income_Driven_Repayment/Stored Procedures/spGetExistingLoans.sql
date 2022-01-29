-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/25/2013
-- Description:	THIS SP WILL GET ALL LOANS FOR A GIVEN APP ID FROM THE LOANS TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingLoans] 

@AppId INT

AS
BEGIN

	SELECT 
		borrower_id As BorrowerId,
		application_id As AppId,
		loan_type AS LoanType,
		loan_seq AS LoanSeq,
		award_id AS AwardId,
		disb_date AS DisbDate
	FROM 
		dbo.Loans
	WHERE
		application_id = @AppId
END