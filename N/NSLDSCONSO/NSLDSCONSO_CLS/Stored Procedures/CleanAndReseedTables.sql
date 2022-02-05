CREATE PROCEDURE [nsldsconso].[CleanAndReseedTables]
AS

	TRUNCATE TABLE
		nsldsconso.BorrowerUnderlyingLoanFunding

	TRUNCATE TABLE
		nsldsconso.BorrowerUnderlyingLoans

	TRUNCATE TABLE
		nsldsconso.BorrowerConsolidationLoans

	DELETE FROM
		nsldsconso.Borrowers
	DBCC CHECKIDENT ([nsldsconso.Borrowers], RESEED, 0); 

	DELETE FROM
		nsldsconso.DataLoadRuns
	DBCC CHECKIDENT ([nsldsconso.DataLoadRuns], RESEED, 0);  


RETURN 0
