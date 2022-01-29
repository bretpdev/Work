-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_PhysicalDamageLossTheft table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetPhysicalDamageLossTheft]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@DataWasEncrypted BIT,
	@DesktopWasDamaged BIT,
	@DesktopWasLost BIT,
	@DesktopWasStolen BIT,
	@LaptopWasDamaged BIT,
	@LaptopWasLost BIT,
	@LaptopWasStolen BIT,
	@MicrofilmWithRecordsContainingPiiWasLost BIT,
	@MicrofilmWithRecordsContainingPiiWasStolen BIT,
	@MobileCommunicationDeviceWasLost BIT,
	@MobileCommunicationDeviceWasStolen BIT,
	@PaperRecordWithPiiWasLost BIT,
	@PaperRecordWithPiiWasStolen BIT,
	@RemovableMediaWithPiiWasLost BIT,
	@RemovableMediaWithPiiWasStolen BIT,
	@WindowOrDoorWasDamaged BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_PhysicalDamageLossTheft WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_PhysicalDamageLossTheft (
				TicketNumber,
				TicketType,
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
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@DataWasEncrypted,
				@DesktopWasDamaged,
				@DesktopWasLost,
				@DesktopWasStolen,
				@LaptopWasDamaged,
				@LaptopWasLost,
				@LaptopWasStolen,
				@MicrofilmWithRecordsContainingPiiWasLost,
				@MicrofilmWithRecordsContainingPiiWasStolen,
				@MobileCommunicationDeviceWasLost,
				@MobileCommunicationDeviceWasStolen,
				@PaperRecordWithPiiWasLost,
				@PaperRecordWithPiiWasStolen,
				@RemovableMediaWithPiiWasLost,
				@RemovableMediaWithPiiWasStolen,
				@WindowOrDoorWasDamaged
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_PhysicalDamageLossTheft
			SET DataWasEncrypted = @DataWasEncrypted,
				DesktopWasDamaged = @DesktopWasDamaged,
				DesktopWasLost = @DesktopWasLost,
				DesktopWasStolen = @DesktopWasStolen,
				LaptopWasDamaged = @LaptopWasDamaged,
				LaptopWasLost = @LaptopWasLost,
				LaptopWasStolen = @LaptopWasStolen,
				MicrofilmWithRecordsContainingPiiWasLost = @MicrofilmWithRecordsContainingPiiWasLost,
				MicrofilmWithRecordsContainingPiiWasStolen = @MicrofilmWithRecordsContainingPiiWasStolen,
				MobileCommunicationDeviceWasLost = @MobileCommunicationDeviceWasLost,
				MobileCommunicationDeviceWasStolen = @MobileCommunicationDeviceWasStolen,
				PaperRecordWithPiiWasLost = @PaperRecordWithPiiWasLost,
				PaperRecordWithPiiWasStolen = @PaperRecordWithPiiWasStolen,
				RemovableMediaWithPiiWasLost = @RemovableMediaWithPiiWasLost,
				RemovableMediaWithPiiWasStolen = @RemovableMediaWithPiiWasStolen,
				WindowOrDoorWasDamaged = @WindowOrDoorWasDamaged
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END