--DECLARE @BeginDate DATE = '2019-11-01'
--DECLARE @EndDate DATE = '2020-03-02'
SELECT
	TD.TrackingDataId,
	TD.AccountNumber,
	TD.FirstName,
	TD.LastName,
	TD.PerformanceCategory,
	TD.PhoneNumber,
	TD.Segment,
	TD.StateCode,
	TD.MaximumDelinquency,
	TD.Age,
	TD.PhoneType,
	TD.ContentType,
	TD.CreatedAt,
	TD.CreatedBy,
	Texts.[status] AS [Status],
	TEXTS.txt_sent,
	Texts.Direction,
	Texts.friendly_from AS [From Number],
	Texts.body AS TextContent,
	IIF(TD.ArcAddProcessingId IS NULL, 'N', 'Y') AS ArcAdded
FROM
	ULS.textcoord.TrackingData TD
	INNER JOIN txt.dbo.twilio_data Texts
		ON Texts.friendly_to = TD.PhoneNumber
		AND Texts.txt_created BETWEEN TD.CreatedAt AND DATEADD(DAY,7,TD.CreatedAt) --Datetimes compared intentionally
		AND CAST(Texts.created_on AS DATE) BETWEEN @BeginDate AND @EndDate
WHERE
	CAST(TD.CreatedAt AS DATE) BETWEEN @BeginDate AND @EndDate
	AND TD.DeletedAt IS NULL