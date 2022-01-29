CREATE PROCEDURE [calls].[GetAllEnabledReasons]
AS

SELECT
	r.ReasonId,
	c.Title as Category,
	r.ReasonText,
	r.Cornerstone,
	r.Uheaa,
	r.Outbound,
	r.Inbound
FROM
	calls.Reasons r
JOIN
	calls.Categories c on c.CategoryId = r.CategoryId
WHERE
	[Enabled] = 1
	
RETURN 0
