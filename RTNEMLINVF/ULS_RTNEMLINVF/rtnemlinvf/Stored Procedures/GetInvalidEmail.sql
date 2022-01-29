CREATE PROCEDURE [rtnemlinvf].[GetInvalidEmail]
AS
	SELECT
		InvalidEmailId,
		Ssn,
		EmailType,
		ReceivedBy [EmailAddress],
		InvalidatedAt,
		ArcAddProcessingId
	FROM
		rtnemlinvf.InvalidEmail
	WHERE
		(
			InvalidatedAt IS NULL
			OR ArcAddProcessingId IS NULL
		)
		AND DeletedAt IS NULL
RETURN 0