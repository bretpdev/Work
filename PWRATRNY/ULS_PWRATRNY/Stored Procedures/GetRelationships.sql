CREATE PROCEDURE [pwratrny].[GetRelationships]
AS

SELECT
	[Description],
	CompassCode,
	OnelinkCode
FROM
	pwratrny.Relationships
WHERE
	Active = 1
	AND DeletedAt IS NULL