CREATE TABLE [dbo].[DAT_ThreatCallerInfo] (
    [TicketNumber]              BIGINT        NOT NULL,
    [TicketType]                VARCHAR (50)  NOT NULL,
    [CallDuration]              VARCHAR (50)  NULL,
    [RecognizedTheCallersVoice] BIT           CONSTRAINT [DF_DAT_ThreatCallerInfo_RecognizedTheCallersVoice] DEFAULT ((0)) NOT NULL,
    [CallerIsFamiliarWithUheaa] BIT           CONSTRAINT [DF_DAT_ThreatCallerInfo_CallerIsFamiliarWithUheaa] DEFAULT ((0)) NOT NULL,
    [FamiliaritySpecifics]      TEXT          NULL,
    [Sex]                       VARCHAR (10)  NULL,
    [Age]                       VARCHAR (10)  NULL,
    [Name]                      VARCHAR (50)  NULL,
    [PhoneNumber]               VARCHAR (20)  NULL,
    [Address]                   VARCHAR (100) NULL,
    [AccountNumber]             VARCHAR (10)  NULL,
    CONSTRAINT [PK_DAT_ThreatCallerInfo] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatCallerInfo_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_ThreatCallerInfo_LST_Sex] FOREIGN KEY ([Sex]) REFERENCES [dbo].[LST_Sex] ([Sex]) ON UPDATE CASCADE
);

