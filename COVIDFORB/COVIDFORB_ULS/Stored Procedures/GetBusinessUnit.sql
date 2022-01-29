CREATE PROCEDURE [covidforb].[GetBusinessUnit]
(
	@BusinessUnitId BIGINT
)
AS

SELECT
	BU.BusinessUnitId,
	BU.BusinessUnit,
	BU.ARC
FROM
	[covidforb].[BusinessUnits] BU
WHERE 
	@BusinessUnitId = BU.BusinessUnitId