CREATE PROCEDURE [hrbridge].[GetBlacklist]
AS
	SELECT
		[UID],
		Destination
	FROM
		hrbridge.BridgeBlacklist
RETURN 0
