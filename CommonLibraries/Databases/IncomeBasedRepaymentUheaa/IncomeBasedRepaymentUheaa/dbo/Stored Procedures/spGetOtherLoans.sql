-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/11/2013
-- Description:	WILL PULL DATA FROM dbo.Other_Loans FOR A GIVEN APPID AND TYPE
-- =============================================
CREATE PROCEDURE [dbo].[spGetOtherLoans]

@AppId INT,
@Type BIT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		application_id AS ApplicationId,
		spouse_indicator AS SpouseIndicator,
		loan_type AS LoanType,
		owner_lender AS OwnerLender,
		outstanding_balance AS OutstandingBalance,
		monthly_pay AS MonthlyPay,
		interest_rate AS Interestrate,
		ffelp AS Ffelp
	FROM
		dbo.Other_Loans
	WHERE	
		application_id = @AppId
		AND spouse_indicator = @Type
END
