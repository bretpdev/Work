CREATE TABLE [dbo].[DAT_ScansProbes] (
    [TicketNumber]                           BIGINT       NOT NULL,
    [TicketType]                             VARCHAR (50) NOT NULL,
    [UnauthorizedProgramOrSnifferDevice]     BIT          CONSTRAINT [DF_DAT_ScansProbes_UnauthorizedProgramOrSnifferDevice] DEFAULT ((0)) NOT NULL,
    [PrioritySystemAlarmOrIndicationFromIds] BIT          CONSTRAINT [DF_DAT_ScansProbes_PrioritySystemAlarmOrIndicationFromIds] DEFAULT ((0)) NOT NULL,
    [UnauthorizedPortScan]                   BIT          CONSTRAINT [DF_DAT_ScansProbes_UnauthorizedPortScan] DEFAULT ((0)) NOT NULL,
    [UnauthorizedVulnerabilityScan]          BIT          CONSTRAINT [DF_DAT_ScansProbes_UnauthorizedVulnerabilityScan] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_ScansProbes] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ScansProbes_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

