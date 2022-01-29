CREATE PROCEDURE [forb].[GetBusinessUnit]
(
	@BusinessUnitId BIGINT
)
AS

SELECT
	BU.BusinessUnitId,
	BU.BusinessUnit,
	BU.ARC
FROM
	[forb].[BusinessUnits] BU
WHERE @BusinessUnitId = BU.BusinessUnitId