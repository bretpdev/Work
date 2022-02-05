CREATE TABLE [dbo].[DAT_Incident] (
    [TicketNumber]                 BIGINT        NOT NULL,
    [TicketType]                   VARCHAR (50)  NOT NULL,
    [Cause]                        VARCHAR (50)  NOT NULL,
    [BorrowerSsnAndDobAreVerified] BIT           CONSTRAINT [DF_DAT_Incident_BorrowerSsnAndDobAreVerified] DEFAULT ((0)) NOT NULL,
    [Priority]                     VARCHAR (10)  NOT NULL,
    [Location]                     VARCHAR (100) NULL,
    [Narrative]                    VARCHAR (MAX) NULL,
    CONSTRAINT [PK_DAT_Incident] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Incident_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_Incident_LST_IncidentCause] FOREIGN KEY ([Cause]) REFERENCES [dbo].[LST_IncidentCause] ([Cause]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_Incident_LST_IncidentPriority] FOREIGN KEY ([Priority]) REFERENCES [dbo].[LST_IncidentPriority] ([Priority]) ON UPDATE CASCADE
);

