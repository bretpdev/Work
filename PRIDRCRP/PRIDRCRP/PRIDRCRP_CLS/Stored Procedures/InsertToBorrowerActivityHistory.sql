CREATE PROCEDURE [pridrcrp].[InsertToBorrowerActivityHistory]
(
	@BorrowerActivityRecord BorrowerActivityRecord READONLY,
	@BorrowerInformationId INT
)
AS

DECLARE @BwrAtyHstOut TABLE
(
	BorrowerActivityId INT,
	BorrowerInformationId INT,
	ActivityDate DATE,
	ActivityDescription VARCHAR(MAX)
)

INSERT INTO [pridrcrp].BorrowerActivityHistory(BorrowerInformationId, ActivityDate, ActivityDescription)
OUTPUT INSERTED.BorrowerActivityId, INSERTED.BorrowerInformationId, INSERTED.ActivityDate, INSERTED.ActivityDescription
	INTO @BwrAtyHstOut
SELECT
	@BorrowerInformationId,
	BAR.[ActivityDate],
	BAR.[ActivityDescription]
FROM
	@BorrowerActivityRecord BAR
	LEFT JOIN [pridrcrp].[BorrowerActivityHistory] BAH
		ON BAR.[ActivityDate] = BAH.ActivityDate
		AND BAR.[ActivityDescription] = BAH.ActivityDescription
		AND @BorrowerInformationId = BAH.BorrowerInformationId
WHERE
	BAH.BorrowerActivityId IS NULL


SELECT
	BorrowerActivityId,
	BorrowerInformationId,
	ActivityDate,
	ActivityDescription
FROM
	@BwrAtyHstOut