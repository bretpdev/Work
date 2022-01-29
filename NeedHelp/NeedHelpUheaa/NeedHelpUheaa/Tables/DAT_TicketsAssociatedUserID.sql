CREATE TABLE [dbo].[DAT_TicketsAssociatedUserID] (
    [Identifier] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Ticket]     BIGINT        NOT NULL,
    [Role]       NVARCHAR (50) NULL,
    [SqlUserId]  INT           NULL,
    CONSTRAINT [PK_NDHP_DAT_UpdateTicketUserIDs] PRIMARY KEY CLUSTERED ([Identifier] ASC) WITH (FILLFACTOR = 90)
);

