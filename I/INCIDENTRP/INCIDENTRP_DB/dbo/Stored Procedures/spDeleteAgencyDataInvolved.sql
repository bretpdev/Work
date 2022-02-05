﻿-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Removes a record from the DAT_AgencyDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteAgencyDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE FROM DAT_AgencyDataInvolved
    WHERE TicketNumber = @TicketNumber
END