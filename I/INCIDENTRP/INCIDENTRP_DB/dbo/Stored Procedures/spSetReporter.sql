-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_Reporter table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetReporter] 
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@SqlUserId INT = NULL,
	@BusinessUnitID INT = NULL,
	@PhoneNumber VARCHAR(20) = NULL,
	@Location VARCHAR(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_Reporter WHERE TicketNumber = @TicketNumber AND TicketType = @TicketType) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_Reporter (
				TicketNumber,
				TicketType,
				SqlUserId,
				BusinessUnitId,
				PhoneNumber,
				Location
			)
			VALUES (
				@TicketNumber,
				@TicketType,
				@SqlUserId,
				@BusinessUnitID,
				@PhoneNumber,
				@Location
			)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_Reporter
			SET SqlUserId = @SqlUserId,
				BusinessUnitId = @BusinessUnitID,
				PhoneNumber = @PhoneNumber,
				Location = @Location
			WHERE TicketNumber = @TicketNumber
				AND TicketType = @TicketType
		END
END