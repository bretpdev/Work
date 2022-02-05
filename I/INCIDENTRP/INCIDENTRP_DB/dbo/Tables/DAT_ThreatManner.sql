CREATE TABLE [dbo].[DAT_ThreatManner] (
    [TicketNumber]     BIGINT       NOT NULL,
    [TicketType]       VARCHAR (50) NOT NULL,
    [Angry]            BIT          CONSTRAINT [DF_DAT_ThreatManner_Angry] DEFAULT ((0)) NOT NULL,
    [BusinessLike]     BIT          CONSTRAINT [DF_DAT_ThreatManner_BusinessLike] DEFAULT ((0)) NOT NULL,
    [Calm]             BIT          CONSTRAINT [DF_DAT_ThreatManner_Calm] DEFAULT ((0)) NOT NULL,
    [Coherent]         BIT          CONSTRAINT [DF_DAT_ThreatManner_Coherent] DEFAULT ((0)) NOT NULL,
    [Deliberate]       BIT          CONSTRAINT [DF_DAT_ThreatManner_Deliberate] DEFAULT ((0)) NOT NULL,
    [Emotional]        BIT          CONSTRAINT [DF_DAT_ThreatManner_Emotional] DEFAULT ((0)) NOT NULL,
    [IllAtEase]        BIT          CONSTRAINT [DF_DAT_ThreatManner_IllAtEase] DEFAULT ((0)) NOT NULL,
    [Incoherent]       BIT          CONSTRAINT [DF_DAT_ThreatManner_Incoherent] DEFAULT ((0)) NOT NULL,
    [Irrational]       BIT          CONSTRAINT [DF_DAT_ThreatManner_Irrational] DEFAULT ((0)) NOT NULL,
    [Laughing]         BIT          CONSTRAINT [DF_DAT_ThreatManner_Laughing] DEFAULT ((0)) NOT NULL,
    [Rational]         BIT          CONSTRAINT [DF_DAT_ThreatManner_Rational] DEFAULT ((0)) NOT NULL,
    [Righteous]        BIT          CONSTRAINT [DF_DAT_ThreatManner_Righteous] DEFAULT ((0)) NOT NULL,
    [Shouting]         BIT          CONSTRAINT [DF_DAT_ThreatManner_Shouting] DEFAULT ((0)) NOT NULL,
    [Slow]             BIT          CONSTRAINT [DF_DAT_ThreatManner_Slow] DEFAULT ((0)) NOT NULL,
    [Other]            BIT          CONSTRAINT [DF_DAT_ThreatManner_Other] DEFAULT ((0)) NOT NULL,
    [OtherDescription] VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_ThreatManner] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatManner_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

