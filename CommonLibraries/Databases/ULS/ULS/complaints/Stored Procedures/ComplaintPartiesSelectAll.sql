CREATE PROCEDURE [complaints].[ComplaintPartiesSelectAll]
AS

	select ComplaintPartyId, PartyName
	  from [complaints].ComplaintParties
	 where DeletedOn IS NULL
RETURN 0