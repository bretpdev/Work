CREATE TABLE [dbo].[DAT_ElectronicMailDelivery] (
    [TicketNumber]                                 BIGINT        NOT NULL,
    [TicketType]                                   VARCHAR (50)  NOT NULL,
    [EmailAddressWasDisclosed]                     BIT           CONSTRAINT [DF_DAT_ElectronicMailDelivery_EmailAddressWasDisclosed] DEFAULT ((0)) NOT NULL,
    [FtpTransmissionWasSentToIncorrectDestination] BIT           CONSTRAINT [DF_DAT_ElectronicMailDelivery_FtpTransmissionWasSentToIncorrectDestination] DEFAULT ((0)) NOT NULL,
    [IncorrectAttachmentContainedPii]              BIT           CONSTRAINT [DF_DAT_ElectronicMailDelivery_IncorrectAttachmentContainedPii] DEFAULT ((0)) NOT NULL,
    [EmailWasDeliveredToIncorrectAddress]          BIT           CONSTRAINT [DF_DAT_ElectronicMailDelivery_EmailWasDeliveredToIncorrectAddress] DEFAULT ((0)) NOT NULL,
    [Detail]                                       VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_DAT_ElectronicMailDelivery] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ElectronicMailDelivery_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

