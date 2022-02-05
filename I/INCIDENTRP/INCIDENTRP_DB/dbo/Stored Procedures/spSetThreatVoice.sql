-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatVoice table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatVoice]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Distinct BIT = 0,
	@Distorted BIT = 0,
	@Fast BIT = 0,
	@High BIT = 0,
	@Hoarse BIT = 0,
	@Lisp BIT = 0,
	@Nasal BIT = 0,
	@Slow BIT = 0,
	@Slurred BIT = 0,
	@Stuttering BIT = 0,
	@Other BIT = 0,
	@OtherDescription VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatVoice WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatVoice (TicketNumber, TicketType, [Distinct], Distorted, [Fast], High, Hoarse, Lisp, Nasal, Slow, Slurred, Stuttering, Other, OtherDescription)
			VALUES (@TicketNumber, 'Threat', @Distinct, @Distorted, @Fast, @High, @Hoarse, @Lisp, @Nasal, @Slow, @Slurred, @Stuttering, @Other, @OtherDescription)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatVoice
			SET [Distinct] = @Distinct,
				Distorted = @Distorted,
				[Fast] = @Fast,
				High = @High,
				Hoarse = @Hoarse,
				Lisp = @Lisp,
				Nasal = @Nasal,
				Slow = @Slow,
				Slurred = @Slurred,
				Stuttering = @Stuttering,
				Other = @Other,
				OtherDescription = @OtherDescription
			WHERE TicketNumber = @TicketNumber
		END
END