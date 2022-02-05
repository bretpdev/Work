-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_AccessControl table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetAccessControl]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@ImproperAccessWasGranted BIT,
	@SystemAccessWasNotTerminatedOrModified BIT,
	@PhysicalAccessWasNotTerminatedOrModified BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_AccessControl WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_AccessControl (
				TicketNumber,
				TicketType,
				ImproperAccessWasGranted,
				SystemAccessWasNotTerminatedOrModified,
				PhysicalAccessWasNotTerminatedOrModified
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@ImproperAccessWasGranted,
				@SystemAccessWasNotTerminatedOrModified,
				@PhysicalAccessWasNotTerminatedOrModified
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_AccessControl
			SET ImproperAccessWasGranted = @ImproperAccessWasGranted,
				SystemAccessWasNotTerminatedOrModified = @SystemAccessWasNotTerminatedOrModified,
				PhysicalAccessWasNotTerminatedOrModified = @PhysicalAccessWasNotTerminatedOrModified
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END