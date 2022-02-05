

CREATE PROCEDURE [dbo].[spNDHP_NewTicket] 
@TicketCode	char (3),
@Requester	nvarchar (50)

AS

DECLARE @TicketNumber	bigint
DECLARE @BU			nvarchar(50)

SET @BU = (SELECT MAX(BusinessUnit) AS BusinessUnit
		FROM dbo.GENR_REF_BU_Agent_Xref
		WHERE WindowsUserID = @Requester
		AND Role = 'Member Of')

INSERT INTO dbo.NDHP_DAT_Tickets (TicketCode, Unit, Status) VALUES (@TicketCode, @BU, 'Submitting')

SET @TicketNumber = @@IDENTITY

INSERT INTO dbo.NDHP_DAT_UpdateTicketUserIDs VALUES (@TicketNumber, 'Requester', @Requester)
INSERT INTO dbo.NDHP_DAT_UpdateTicketUserIDs VALUES (@TicketNumber, 'Court', @Requester)

EXEC spNDHP_StatusUpdate @TicketNumber, 'Submitting',@Requester

SELECT CAST(@TicketNumber AS BIGINT)
RETURN @TicketNumber