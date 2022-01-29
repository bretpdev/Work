-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_PhysicalDamageLossTheft table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetPhysicalDamageLossTheft]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DataWasEncrypted,
		DesktopWasDamaged,
		DesktopWasLost,
		DesktopWasStolen,
		LaptopWasDamaged,
		LaptopWasLost,
		LaptopWasStolen,
		MicrofilmWithRecordsContainingPiiWasLost,
		MicrofilmWithRecordsContainingPiiWasStolen,
		MobileCommunicationDeviceWasLost,
		MobileCommunicationDeviceWasStolen,
		PaperRecordWithPiiWasLost,
		PaperRecordWithPiiWasStolen,
		RemovableMediaWithPiiWasLost,
		RemovableMediaWithPiiWasStolen,
		WindowOrDoorWasDamaged
	FROM DAT_PhysicalDamageLossTheft
	WHERE TicketNumber = @TicketNumber
END