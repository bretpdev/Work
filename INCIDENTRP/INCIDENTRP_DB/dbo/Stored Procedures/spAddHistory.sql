-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Adds a record to the DATHistory table.
-- =============================================
CREATE PROCEDURE [dbo].[spAddHistory]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@UpdateDateTime DATETIME,
	@SqlUserId INT,
	@Status VARCHAR(50),
	@StatusChangeDescription TEXT = NULL,
	@UpdateText TEXT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO DATHistory (
		TicketNumber,
		TicketType,
		UpdateDateTime,
		SqlUserId,
		[Status],
		StatusChangeDescription,
		[UpdateText]
	)
	VALUES (
		@TicketNumber,
		@TicketType,
		@UpdateDateTime,
		@SqlUserId,
		@Status,
		@StatusChangeDescription,
		@UpdateText
	)
END