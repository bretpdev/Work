CREATE TABLE [dbo].[OLD_NDHP_DAT_UpdateTicketUserIDs] (
    [Identifier] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Ticket]     BIGINT        NOT NULL,
    [Role]       NVARCHAR (50) NULL,
    [UserID]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_NDHP_DAT_UpdateTicketUserIDs] PRIMARY KEY CLUSTERED ([Identifier] ASC),
    CONSTRAINT [FK_NDHP_DAT_UpdateTicketUserIDs_NDHP_DAT_Tickets] FOREIGN KEY ([Ticket]) REFERENCES [dbo].[OLD_NDHP_DAT_Tickets] ([Ticket]) ON UPDATE CASCADE,
    CONSTRAINT [FK_NDHP_DAT_UpdateTicketUserIDs_SYSA_LST_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

