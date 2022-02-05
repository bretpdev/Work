CREATE PROCEDURE [pridrcrp].[InsertToMonetaryHistoryToBorrowerInformation]
(
	@BorrowerInformationId INT,
	@MonetaryHistoryId INT
)
AS

DECLARE @Found INT =
(
	SELECT
		BorrowerInformationId
	FROM
		[pridrcrp].[MonetaryHistoryToBorrowerInformation]
	WHERE
		[BorrowerInformationId] = @BorrowerInformationId
		AND [MonetaryHistoryId] = @MonetaryHistoryId
)

IF @Found IS NULL
	BEGIN
		INSERT INTO [pridrcrp].[MonetaryHistoryToBorrowerInformation]
			([BorrowerInformationId],
			[MonetaryHistoryId])
		VALUES
			(@BorrowerInformationId,
			@MonetaryHistoryId)
	END