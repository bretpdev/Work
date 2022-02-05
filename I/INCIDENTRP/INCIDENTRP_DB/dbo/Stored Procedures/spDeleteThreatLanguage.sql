﻿-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Removes a record from the DAT_ThreatLanguage table.
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteThreatLanguage]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM DAT_ThreatLanguage
	WHERE TicketNumber = @TicketNumber
END