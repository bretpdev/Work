CREATE TABLE [dbo].[DAT_UnauthorizedSystemAccess] (
    [TicketNumber]                        BIGINT       NOT NULL,
    [TicketType]                          VARCHAR (50) NOT NULL,
    [SuspiciousEntryInSystemOrNetworkLog] BIT          CONSTRAINT [DF_DAT_UnauthorizedSystemAccess_SuspiciousEntryInSystemOrNetworkLog] DEFAULT ((0)) NOT NULL,
    [SystemAccountDiscrepancy]            BIT          CONSTRAINT [DF_DAT_UnauthorizedSystemAccess_SystemAccountDiscrepancy] DEFAULT ((0)) NOT NULL,
    [UnauthorizedUseOfUserCredentials]    BIT          CONSTRAINT [DF_DAT_UnauthorizedSystemAccess_UnauthorizedUseOfUserCredentials] DEFAULT ((0)) NOT NULL,
    [UnexplainedNewUserAccount]           BIT          CONSTRAINT [DF_DAT_UnauthorizedSystemAccess_UnexplainedNewUserAccount] DEFAULT ((0)) NOT NULL,
    [UnusualTimeOfUsage]                  BIT          CONSTRAINT [DF_DAT_UnauthorizedSystemAccess_UnusualTimeOfUsage] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_UnauthorizedSystemAccess] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_UnauthorizedSystemAccess_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

