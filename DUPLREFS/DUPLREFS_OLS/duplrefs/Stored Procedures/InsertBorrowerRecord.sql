CREATE PROCEDURE [duplrefs].[InsertBorrowerRecord]
	@AccountNumber VARCHAR(10),
	@UserId VARCHAR(8)

AS

BEGIN

	DECLARE @BorrowerAlreadyExists BIT =
	(
		SELECT
			CASE WHEN COUNT(1) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			END AS BorrowerAlreadyExists
		FROM
			duplrefs.BorrowerQueue BQ
		WHERE
			BQ.AccountNumber = @AccountNumber
			AND BQ.DeletedAt IS NULL
			AND BQ.ProcessedAt IS NULL
	)
	
	IF (@BorrowerAlreadyExists = 0)
		BEGIN
			INSERT INTO duplrefs.BorrowerQueue(AccountNumber,UserId)
			VALUES (@AccountNumber,@UserId)

			SELECT SCOPE_IDENTITY() 
		END
	ELSE
		SELECT 0; --An impossible value that will alert script that no insertion occurred

END
