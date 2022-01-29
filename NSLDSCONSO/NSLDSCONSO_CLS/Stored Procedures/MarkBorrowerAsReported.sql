CREATE PROCEDURE [nsldsconso].[MarkBorrowerAsReported]
	@BorrowerId INT
AS
	
	UPDATE
		b
	SET
		b.ReportedToNsldsOn = GETDATE(),
		b.ReportedToNsldsBy = SYSTEM_USER
	FROM
		nsldsconso.Borrowers b
	WHERE
		b.BorrowerId = @BorrowerId

RETURN 0
