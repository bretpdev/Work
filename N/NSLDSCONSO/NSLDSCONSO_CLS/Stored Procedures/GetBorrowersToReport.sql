CREATE PROCEDURE [nsldsconso].[GetBorrowersToReport]
AS

	SELECT
		B.BorrowerId,
		B.Ssn,
		CASE WHEN LN10.BF_SSN IS NOT NULL THEN 1 ELSE 0 END [HasReleasedLoans]
	FROM
		nsldsconso.Borrowers B
	LEFT JOIN
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			CDW.dbo.LN10_LON 
		WHERE
			LC_STA_LON10 = 'R'
	) LN10
		ON LN10.BF_SSN = B.Ssn
	WHERE
		B.ReportedToNsldsOn IS NULL
		AND
		B.AddedOn > DATEADD(MONTH, -2, GETDATE())
		AND
		B.InactivatedOn IS NULL

RETURN 0
