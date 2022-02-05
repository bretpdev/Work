CREATE TABLE [dbo].[DAT_PhysicalDamageLossTheft] (
    [TicketNumber]                               BIGINT       NOT NULL,
    [TicketType]                                 VARCHAR (50) NOT NULL,
    [DataWasEncrypted]                           BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_DataWasEncrypted] DEFAULT ((0)) NOT NULL,
    [DesktopWasDamaged]                          BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_DesktopWasDamaged] DEFAULT ((0)) NOT NULL,
    [DesktopWasLost]                             BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_DesktopWasLost] DEFAULT ((0)) NOT NULL,
    [DesktopWasStolen]                           BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_DesktopWasStolen] DEFAULT ((0)) NOT NULL,
    [LaptopWasDamaged]                           BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_LaptopWasDamaged] DEFAULT ((0)) NOT NULL,
    [LaptopWasLost]                              BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_LaptopWasLost] DEFAULT ((0)) NOT NULL,
    [LaptopWasStolen]                            BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_LaptopWasStolen] DEFAULT ((0)) NOT NULL,
    [MicrofilmWithRecordsContainingPiiWasLost]   BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_MicrofilmWithRecordsContainingPiiWasLost] DEFAULT ((0)) NOT NULL,
    [MicrofilmWithRecordsContainingPiiWasStolen] BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_MicrofilmWithRecordsContainingPiiWasStolen] DEFAULT ((0)) NOT NULL,
    [MobileCommunicationDeviceWasLost]           BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_MobileCommunicationDeviceWasLost] DEFAULT ((0)) NOT NULL,
    [MobileCommunicationDeviceWasStolen]         BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_MobileCommunicationDeviceWasStolen] DEFAULT ((0)) NOT NULL,
    [PaperRecordWithPiiWasLost]                  BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_PaperRecordWithPiiWasLost] DEFAULT ((0)) NOT NULL,
    [PaperRecordWithPiiWasStolen]                BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_PaperRecordWithPiiWasStolen] DEFAULT ((0)) NOT NULL,
    [RemovableMediaWithPiiWasLost]               BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_RemovableMediaWithPiiWasLost] DEFAULT ((0)) NOT NULL,
    [RemovableMediaWithPiiWasStolen]             BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_RemovableMediaWithPiiWasStolen] DEFAULT ((0)) NOT NULL,
    [WindowOrDoorWasDamaged]                     BIT          CONSTRAINT [DF_DAT_PhysicalDamageLossTheft_WindowOrDoorWasDamaged] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_PhysicalDamageLossTheft] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_PhysicalDamageLossTheft_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

