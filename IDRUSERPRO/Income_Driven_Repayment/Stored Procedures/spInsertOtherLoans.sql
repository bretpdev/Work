﻿-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/11/2013
-- Description:	WILL INSERT DATA INTO THE dbo.Other_Loans TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spInsertOtherLoans]
	@ApplicationId INT,
	@SpouseIndicator BIT,
	@LoanType VARCHAR(10),
	@OwnerLender VARCHAR(50),
	@OutstandingBalance MONEY,
	@OutstandingInterest MONEY,
	@MonthlyPay MONEY = null,
	@Interestrate DECIMAL(18,2) = null,
	@Ffelp BIT
AS
BEGIN

	INSERT INTO dbo.Other_Loans(application_id,spouse_indicator,loan_type,owner_lender,outstanding_balance,outstanding_interest,monthly_pay,interest_rate,ffelp)
	VALUES(@ApplicationId,@SpouseIndicator,@LoanType,@OwnerLender,@OutstandingBalance,@OutstandingInterest,@MonthlyPay,@Interestrate,@Ffelp)

END