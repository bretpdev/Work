-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatManner table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatManner]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Angry BIT = 0,
	@BusinessLike BIT = 0,
	@Calm BIT = 0,
	@Coherent BIT = 0,
	@Deliberate BIT = 0,
	@Emotional BIT = 0,
	@IllAtEase BIT = 0,
	@Incoherent BIT = 0,
	@Irrational BIT = 0,
	@Laughing BIT = 0,
	@Rational BIT = 0,
	@Righteous BIT = 0,
	@Shouting BIT = 0,
	@Slow BIT = 0,
	@Other BIT = 0,
	@OtherDescription VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatManner WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatManner (TicketNumber, TicketType, Angry, BusinessLike, Calm, Coherent, Deliberate, Emotional, IllAtEase, Incoherent, Irrational, Laughing, Rational, Righteous, Shouting, Slow, Other, OtherDescription)
			VALUES (@TicketNumber, 'Threat', @Angry, @BusinessLike, @Calm, @Coherent, @Deliberate, @Emotional, @IllAtEase, @Incoherent, @Irrational, @Laughing, @Rational, @Righteous, @Shouting, @Slow, @Other, @OtherDescription)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatManner
			SET Angry = @Angry,
				BusinessLike = @BusinessLike,
				Calm = @Calm,
				Coherent = @Coherent,
				Deliberate = @Deliberate,
				Emotional = @Emotional,
				IllAtEase = @IllAtEase,
				Incoherent = @Incoherent,
				Irrational = @Irrational,
				Laughing = @Laughing,
				Rational = @Rational,
				Righteous = @Righteous,
				Shouting = @Shouting,
				Slow = @Slow,
				Other = @Other,
				OtherDescription = @OtherDescription
			WHERE TicketNumber = @TicketNumber
		END
END