CREATE TABLE [dbo].[DAT_SystemOrNetworkUnavailable] (
    [TicketNumber]                BIGINT       NOT NULL,
    [TicketType]                  VARCHAR (50) NOT NULL,
    [DenialOrDisruptionOfService] BIT          CONSTRAINT [DF_DAT_SystemOrNetworkUnavailable_DenialOrDisruptionOfService] DEFAULT ((0)) NOT NULL,
    [UnableToLogIntoAccount]      BIT          CONSTRAINT [DF_DAT_SystemOrNetworkUnavailable_UnableToLogIntoAccount] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_SystemOrNetworkUnavailable] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_SystemOrNetworkUnavailable_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

