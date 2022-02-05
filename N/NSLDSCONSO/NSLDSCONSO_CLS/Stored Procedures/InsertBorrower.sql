CREATE PROCEDURE [nsldsconso].[InsertBorrower]
	@DataLoadRunId INT,
	@Ssn CHAR(9),
	@Name VARCHAR(100),
	@DateOfBirth DATE,
	@FileName VARCHAR(50),
	@BorrowerConsolidationLoans BorrowerConsolidationLoansTT READONLY,
	@BorrowerUnderlyingLoans BorrowerUnderlyingLoansTT READONLY,
	@BorrowerUnderlyingLoanFunding BorrowerUnderlyingLoanFundingTT READONLY
AS
	BEGIN TRANSACTION

	INSERT INTO
		nsldsconso.Borrowers (DataLoadRunId, Ssn, Name, DateOfBirth, FileName)
	VALUES
		(@DataLoadRunId, @Ssn, @Name, @DateOfBirth, @FileName)

	DECLARE @BorrowerId INT = CAST(SCOPE_IDENTITY() AS INT)

	INSERT INTO
		nsldsconso.BorrowerConsolidationLoans 
		(
			[BorrowerId], [NewLoanId], [GrossAmount], [InterestRate], [RebateAmount], [NewGrossAmountSubsidized], 
			[NewGrossAmountUnsubsidized], [NewInterestRate], [NewRebateSubsidized], [NewRebateUnsubsidized]
		)
	SELECT
		@BorrowerId, [NewLoanId], [GrossAmount], [InterestRate], [RebateAmount], [NewGrossAmountSubsidized], 
		[NewGrossAmountUnsubsidized], [NewInterestRate], [NewRebateSubsidized], [NewRebateUnsubsidized]
	FROM
		@BorrowerConsolidationLoans

	INSERT INTO
		nsldsconso.BorrowerUnderlyingLoans
		(
			[BorrowerId], [NewLoanId], [UnderlyingLoanId], [NsldsLabel], [LoanType], [FirstDisbursement], [LoanStatus], [JointConsolidationIndicator],
			[ParentPlusLoanFlag], [ForcedIdrFlag], [IdrStartDate], [EconomicHardshipDefermentDaysUsed], [LossOfSubsidyStatus], [LossOfSubsidyStatusDate],
			[InterestRate], [UnderlyingLoanBalance]
		)
	SELECT
		@BorrowerId, [NewLoanId], [UnderlyingLoanId], [NsldsLabel], [LoanType], [FirstDisbursement], [LoanStatus], [JointConsolidationIndicator],
		[ParentPlusLoanFlag], [ForcedIdrFlag], [IdrStartDate], [EconomicHardshipDefermentDaysUsed], [LossOfSubsidyStatus], [LossOfSubsidyStatusDate],
		[InterestRate], [UnderlyingLoanBalance]
	FROM
		@BorrowerUnderlyingLoans

	INSERT INTO
		nsldsconso.BorrowerUnderlyingLoanFunding
		(
			[BorrowerId], [UnderlyingLoanId], [LoanType], [AmountType], [TotalAmount], [RebateAmount], [DisbursementDate], 
			[DisbursementNumber], [DisbursementSequenceNumber],	[DateFunded], [LoanHolderId], [Comments]
		)
	SELECT
		@BorrowerId, [UnderlyingLoanId], [LoanType], [AmountType], [TotalAmount], [RebateAmount], [DisbursementDate], 
		[DisbursementNumber], [DisbursementSequenceNumber],	[DateFunded], [LoanHolderId], [Comments]
	FROM
		@BorrowerUnderlyingLoanFunding

	IF @@ERROR = 0
		COMMIT TRANSACTION
	ELSE
		RAISERROR('Couldn''t insert borrower ',16,2, @Ssn)

RETURN 0
