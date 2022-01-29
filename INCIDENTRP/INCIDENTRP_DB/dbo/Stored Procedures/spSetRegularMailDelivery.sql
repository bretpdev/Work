-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_RegularMailDelivery table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetRegularMailDelivery]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Problem VARCHAR(20),
	@Address1 VARCHAR(50),
	@Address2 VARCHAR(50) = NULL,
	@City VARCHAR(50),
	@State CHAR(2),
	@Zip VARCHAR(10),
	@TrackingNumber VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_RegularMailDelivery WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_RegularMailDelivery (
				TicketNumber,
				TicketType,
				Problem,
				Address1,
				Address2,
				City,
				[State],
				Zip,
				TrackingNumber
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@Problem,
				@Address1,
				@Address2,
				@City,
				@State,
				@Zip,
				@TrackingNumber
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_RegularMailDelivery
			SET Problem = @Problem,
				Address1 = @Address1,
				Address2 = @Address2,
				City = @City,
				[State] = @State,
				Zip = @Zip,
				TrackingNumber = @TrackingNumber
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END