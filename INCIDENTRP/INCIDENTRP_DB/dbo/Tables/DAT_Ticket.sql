CREATE TABLE [dbo].[DAT_Ticket] (
    [TicketNumber]     BIGINT        NOT NULL,
    [TicketType]       VARCHAR (50)  NOT NULL,
    [IncidentDateTime] DATETIME      NOT NULL,
    [CreateDateTime]   DATETIME      CONSTRAINT [DF_DAT_Ticket_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [Requester]        INT           NOT NULL,
    [FunctionalArea]   VARCHAR (100) CONSTRAINT [DF_DAT_Ticket_FunctionalArea_1] DEFAULT ('Security') NOT NULL,
    [Priority]         INT           CONSTRAINT [DF_DAT_Ticket_Priority_1] DEFAULT ((0)) NOT NULL,
    [Status]           VARCHAR (50)  NOT NULL,
    [Court]            INT           NULL,
    [AssignedTo]       INT           NULL,
    CONSTRAINT [PK_DAT_Ticket] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC)
);

