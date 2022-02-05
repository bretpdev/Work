CREATE PROCEDURE [complaints].[ComplaintPartiesSelectAll]
AS

	SELECT
		ComplaintPartyId,
		PartyName
	FROM
		[complaints].ComplaintParties
	WHERE
		DeletedOn IS NULL

RETURN 0