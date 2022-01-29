CREATE PROCEDURE [idrxmldata].[InsertNewXmlLoans]
	@BorrowerId INT,
	@ApplicationId INT
AS
	INSERT INTO dbo.Loans(borrower_id, application_id, loan_type, award_id, loan_seq, disb_date)
	SELECT
		@BorrowerId,
		@ApplicationId,
		LN10.IC_LON_PGM,
		ISNULL(LF_LON_ALT+ RIGHT('000' + CAST(LN_LON_ALT_SEQ AS VARCHAR(3)), 3),''),
		RIGHT('0000' +CAST(LN10.LN_SEQ AS VARCHAR(10)), 4),
		CONVERT(VARCHAR(10),CAST(LN10.LD_LON_1_DSB AS DATETIME),1)
	FROM 
		IncomeBasedRepaymentUheaa..Borrowers B
		INNER JOIN UDW..LN10_LON LN10
			ON B.SSN = LN10.BF_SSN
		LEFT OUTER JOIN IncomeBasedRepaymentUheaa..Loans L
			ON L.loan_seq = RIGHT('0000' +CAST(LN10.LN_SEQ AS VARCHAR(10)), 4)
			AND L.disb_date = CONVERT(VARCHAR(10),CAST(LN10.LD_LON_1_DSB AS DATETIME),1)
			AND L.application_id = @ApplicationId
	WHERE
		B.borrower_id = @BorrowerId
		AND L.application_id IS NULL

