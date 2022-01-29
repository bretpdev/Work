CREATE TABLE [dbo].[DAT_CanceledTickets] (
    [SqlUserId]      INT          NOT NULL,
    [TicketType]     VARCHAR (50) NOT NULL,
    [CreateDateTime] DATETIME     NOT NULL,
    [CancelDateTime] DATETIME     NOT NULL,
    [AccountNumber]  VARCHAR (10) NULL,
    CONSTRAINT [PK_DAT_CanceledTickets_1] PRIMARY KEY CLUSTERED ([SqlUserId] ASC, [TicketType] ASC, [CreateDateTime] ASC)
);

