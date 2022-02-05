-- =============================================
-- Author:		Daren Beattie
-- Create date: September 6, 2011
-- Description:	Inserts or updates a record in the Ticket table corresponding to the ticket number and type.
-- =============================================
CREATE PROCEDURE [dbo].[spSetTicket]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT = 0,
	@TicketType VARCHAR(50),
	@IncidentDateTime DATETIME,
	@CreateDateTime DATETIME,
	@Requester INT,
	@FunctionalArea VARCHAR(100),
	@Priority INT,
	@Status VARCHAR(50),
	@Court INT = NULL,
	@AssignedTo INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @TicketNumber = 0
		-- Get the next available ticket number for the given ticket type and insert a new record.
		BEGIN
			SET @TicketNumber = (SELECT COALESCE(MAX(TicketNumber), 0) + 1 FROM DAT_Ticket WHERE TicketType = @TicketType)
			INSERT INTO DAT_Ticket (
				TicketNumber,
				TicketType,
				IncidentDateTime,
				CreateDateTime,
				Requester,
				FunctionalArea,
				Priority,
				[Status],
				Court,
				AssignedTo
			)
			VALUES (
				@TicketNumber,
				@TicketType,
				@IncidentDateTime,
				@CreateDateTime,
				@Requester,
				@FunctionalArea,
				@Priority,
				@Status,
				@Court,
				@AssignedTo
			)
		END
	ELSE
		-- Update the existing ticket.
		BEGIN
			UPDATE DAT_Ticket
			SET
				IncidentDateTime = @IncidentDateTime,
				CreateDateTime = @CreateDateTime,
				Requester = @Requester,
				FunctionalArea = @FunctionalArea,
				Priority = @Priority,
				[Status] = @Status,
				Court = @Court,
				AssignedTo = @AssignedTo
			WHERE TicketNumber = @TicketNumber
			AND TicketType = @TicketType
		END

	SELECT @TicketNumber
END