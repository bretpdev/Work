CREATE PROCEDURE [payhistlpp].[GetUnprocessedCount]
	@RunId INT
AS
	SELECT
		COUNT(*)
	FROM
		ULS.payhistlpp.Accounts
	WHERE
		@RunId = @RunId
		AND ProcessedAt IS NULL
		AND DeletedAt IS NULL