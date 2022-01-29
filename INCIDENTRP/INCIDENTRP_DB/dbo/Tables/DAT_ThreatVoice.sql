CREATE TABLE [dbo].[DAT_ThreatVoice] (
    [TicketNumber]     BIGINT       NOT NULL,
    [TicketType]       VARCHAR (50) NOT NULL,
    [Distinct]         BIT          CONSTRAINT [DF_DAT_ThreatVoice_Distinct] DEFAULT ((0)) NOT NULL,
    [Distorted]        BIT          CONSTRAINT [DF_DAT_ThreatVoice_Distorted] DEFAULT ((0)) NOT NULL,
    [Fast]             BIT          CONSTRAINT [DF_DAT_ThreatVoice_Fast] DEFAULT ((0)) NOT NULL,
    [High]             BIT          CONSTRAINT [DF_DAT_ThreatVoice_High] DEFAULT ((0)) NOT NULL,
    [Hoarse]           BIT          CONSTRAINT [DF_DAT_ThreatVoice_Hoarse] DEFAULT ((0)) NOT NULL,
    [Lisp]             BIT          CONSTRAINT [DF_DAT_ThreatVoice_Lisp] DEFAULT ((0)) NOT NULL,
    [Nasal]            BIT          CONSTRAINT [DF_DAT_ThreatVoice_Nasal] DEFAULT ((0)) NOT NULL,
    [Slow]             BIT          CONSTRAINT [DF_DAT_ThreatVoice_Slow] DEFAULT ((0)) NOT NULL,
    [Slurred]          BIT          CONSTRAINT [DF_DAT_ThreatVoice_Slurred] DEFAULT ((0)) NOT NULL,
    [Stuttering]       BIT          CONSTRAINT [DF_DAT_ThreatVoice_Stuttering] DEFAULT ((0)) NOT NULL,
    [Other]            BIT          CONSTRAINT [DF_DAT_ThreatVoice_Other] DEFAULT ((0)) NOT NULL,
    [OtherDescription] VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_ThreatVoice] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatVoice_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

