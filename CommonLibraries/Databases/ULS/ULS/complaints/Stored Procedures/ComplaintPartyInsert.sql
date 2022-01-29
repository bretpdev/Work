CREATE PROCEDURE [complaints].[ComplaintPartyInsert]
	@ComplaintPartyName nvarchar(100)
AS

	insert into [complaints].[ComplaintParties] (PartyName)
	values (@ComplaintPartyName)

RETURN 0