CREATE TABLE [dbo].[DAT_ThreatLanguage] (
    [TicketNumber]     BIGINT       NOT NULL,
    [TicketType]       VARCHAR (50) NOT NULL,
    [Educated]         BIT          CONSTRAINT [DF_DAT_ThreatLanguage_Educated] DEFAULT ((0)) NOT NULL,
    [Uneducated]       BIT          CONSTRAINT [DF_DAT_ThreatLanguage_Uneducated] DEFAULT ((0)) NOT NULL,
    [FoulOrProfane]    BIT          CONSTRAINT [DF_DAT_ThreatLanguage_FoulOrProfane] DEFAULT ((0)) NOT NULL,
    [Other]            BIT          CONSTRAINT [DF_DAT_ThreatLanguage_Other] DEFAULT ((0)) NOT NULL,
    [OtherDescription] VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_ThreatLanguage] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatLanguage_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

