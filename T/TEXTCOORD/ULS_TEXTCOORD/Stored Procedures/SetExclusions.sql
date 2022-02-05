CREATE PROCEDURE textcoord.SetExclusions
	@SearchResults SearchResults READONLY
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--Inserts Account number into Nobles Exclusion table (live) and ULS.textcoord._Exclusions(test / name pending) so they dont get called again
--Call on export button click
DELETE FROM ULS.textcoord._Exclusions;
INSERT INTO ULS.textcoord._Exclusions(AccountNumber)
SELECT 
	AccountNumber
FROM
	@SearchResults
END