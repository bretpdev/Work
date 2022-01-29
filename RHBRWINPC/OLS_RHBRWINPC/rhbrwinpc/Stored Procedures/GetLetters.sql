CREATE PROCEDURE [rhbrwinpc].[GetLetters]
AS
	SELECT
		LettersId,
		AccountNumber,
		FirstName,
		LastName,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Country,
		PrintedAt,
		ArcAddProcessingId
	FROM
		[rhbrwinpc].Letters
	WHERE
		DeletedAt IS NULL
		AND 
		(
			PrintedAt IS NULL
			OR ArcAddProcessingId IS NULL
		)