-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_ScansProbes table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetScansProbes]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@UnauthorizedProgramOrSnifferDevice BIT,
	@PrioritySystemAlarmOrIndicationFromIds BIT,
	@UnauthorizedPortScan BIT,
	@UnauthorizedVulnerabilityScan BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ScansProbes WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_ScansProbes (
				TicketNumber,
				TicketType,
				UnauthorizedProgramOrSnifferDevice,
				PrioritySystemAlarmOrIndicationFromIds,
				UnauthorizedPortScan,
				UnauthorizedVulnerabilityScan
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@UnauthorizedProgramOrSnifferDevice,
				@PrioritySystemAlarmOrIndicationFromIds,
				@UnauthorizedPortScan,
				@UnauthorizedVulnerabilityScan
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_ScansProbes
			SET UnauthorizedProgramOrSnifferDevice = @UnauthorizedProgramOrSnifferDevice,
				PrioritySystemAlarmOrIndicationFromIds = @PrioritySystemAlarmOrIndicationFromIds,
				UnauthorizedPortScan = @UnauthorizedPortScan,
				UnauthorizedVulnerabilityScan = @UnauthorizedVulnerabilityScan
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END