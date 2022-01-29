CREATE PROCEDURE [nsldsconso].[GetAllReportedNsldsLabels]
AS

	SELECT
		BUL.NsldsLabel
	FROM
		nsldsconso.BorrowerUnderlyingLoans BUL
	INNER JOIN
		nsldsconso.Borrowers B on B.BorrowerId = BUL.BorrowerId
	WHERE
		B.ReportedToNsldsOn IS NOT NULL

RETURN 0
