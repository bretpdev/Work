CREATE PROCEDURE [complaints].[ComplaintPartyInsert]
	@ComplaintPartyName NVARCHAR(100)
AS

	INSERT INTO [complaints].[ComplaintParties] (PartyName)
	VALUES (@ComplaintPartyName)

RETURN 0