CREATE PROCEDURE textcoord.ExportFile
	@SearchResults SearchResults READONLY,
	@ContentType VARCHAR(20)
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

INSERT INTO textcoord.TrackingData(AccountNumber, FirstName, LastName, PerformanceCategory, PhoneNumber, Segment, StateCode, MaximumDelinquency, Age, PhoneType, ContentType)
SELECT 
	S.AccountNumber,
	S.FirstName,
	S.LastName,
	S.PerformanceCategory,
	S.PhoneNumber,
	S.Segment,
	S.StateCode,
	S.MaximumDelinquency,
	S.Age,
	S.PhoneType,
	@ContentType
FROM
	@SearchResults S
	LEFT JOIN textcoord.UheaaApp1 CallsToday
		ON CallsToday.lm_filler2 = S.AccountNumber
WHERE
	CallsToday.lm_filler2 IS NULL --Didnt have a call today

SELECT
	S.PhoneNumber AS FriendlyTo,
	@ContentType AS ContentType,
	S.FirstName AS FirstName
FROM
	@SearchResults S
	LEFT JOIN textcoord.UheaaApp1 CallsToday
		ON CallsToday.lm_filler2 = S.AccountNumber
WHERE
	CallsToday.lm_filler2 IS NULL --Didnt have a call today
END