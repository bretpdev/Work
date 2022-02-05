CREATE TABLE [dbo].[DAT_UnauthorizedPhysicalAccess] (
    [TicketNumber]                BIGINT       NOT NULL,
    [TicketType]                  VARCHAR (50) NOT NULL,
    [AccessAccountingDiscrepancy] BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_AccessAccountingDiscrepancy] DEFAULT ((0)) NOT NULL,
    [BuildingBreakIn]             BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_BuildingBreakIn] DEFAULT ((0)) NOT NULL,
    [Piggybacking]                BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_Piggybacking] DEFAULT ((0)) NOT NULL,
    [SuspiciousEntryInAccessLog]  BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_SuspiciousEntryInAccessLog] DEFAULT ((0)) NOT NULL,
    [SuspiciousEntryInVideoLog]   BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_SuspiciousEntryInVideoLog] DEFAULT ((0)) NOT NULL,
    [UnauthorizedUseOfKeycard]    BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_UnauthorizedUseOfKeycard] DEFAULT ((0)) NOT NULL,
    [UnexplainedNewKeycard]       BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_UnexplainedNewKeycard] DEFAULT ((0)) NOT NULL,
    [UnusualTimeOfUsage]          BIT          CONSTRAINT [DF_DAT_UnauthorizedPhysicalAccess_UnusualTimeOfUsage] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_UnauthorizedPhysicalAccess] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_UnauthorizedPhysicalAccess_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

