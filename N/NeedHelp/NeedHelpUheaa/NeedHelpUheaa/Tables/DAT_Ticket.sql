CREATE TABLE [dbo].[DAT_Ticket] (
    [Ticket]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [TicketCode]           VARCHAR (50)   NOT NULL,
    [Subject]              VARCHAR (50)   CONSTRAINT [DF_NDHP_DAT_Tickets_Subject] DEFAULT ('') NULL,
    [Requested]            DATETIME       CONSTRAINT [DF_NDHP_DAT_Tickets_Requested] DEFAULT (getdate()) NULL,
    [Unit]                 INT            NULL,
    [Area]                 VARCHAR (100)  CONSTRAINT [DF_NDHP_DAT_Tickets_Area] DEFAULT ('') NULL,
    [Required]             DATETIME       NULL,
    [Issue]                TEXT           CONSTRAINT [DF_NDHP_DAT_Tickets_Issue] DEFAULT ('') NULL,
    [ResolutionCause]      VARCHAR (50)   CONSTRAINT [DF_NDHP_DAT_Tickets_Resolution] DEFAULT ('') NULL,
    [ResolutionFix]        TEXT           CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionFix] DEFAULT ('') NULL,
    [ResolutionPrevention] TEXT           CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionPreventiion] DEFAULT ('') NULL,
    [Status]               VARCHAR (50)   CONSTRAINT [DF_NDHP_DAT_Tickets_Status] DEFAULT ('Submitting') NULL,
    [StatusDate]           DATETIME       CONSTRAINT [DF_NDHP_DAT_Tickets_StatusDate] DEFAULT (getdate()) NULL,
    [CourtDate]            DATETIME       CONSTRAINT [DF_NDHP_DAT_Tickets_CourtDate] DEFAULT (getdate()) NULL,
    [IssueUpdate]          TEXT           CONSTRAINT [DF_NDHP_DAT_Tickets_IssueUpdate] DEFAULT ('') NULL,
    [History]              TEXT           CONSTRAINT [DF_NDHP_DAT_Tickets_History] DEFAULT ('') NULL,
    [PreviousStatus]       VARCHAR (50)   CONSTRAINT [DF_NDHP_DAT_Tickets_PreviousStatus] DEFAULT ('') NULL,
    [UrgencyOption]        VARCHAR (200)  CONSTRAINT [DF_NDHP_DAT_Tickets_Urgency] DEFAULT ('') NULL,
    [CatOption]            VARCHAR (200)  CONSTRAINT [DF_NDHP_DAT_Tickets_Cat] DEFAULT ('') NULL,
    [Priority]             SMALLINT       NULL,
    [LastUpdated]          DATETIME       CONSTRAINT [DF_NDHP_DAT_Tickets_LastUpdated] DEFAULT (getdate()) NULL,
    [CCCIssue]             VARCHAR (20)   CONSTRAINT [DF_NDHP_DAT_Tickets_CCCIssue] DEFAULT ('') NULL,
    [RequestProjectNum]    VARCHAR (50)   CONSTRAINT [DF_NDHP_DAT_Tickets_RequestProjectNum] DEFAULT ('') NULL,
    [Comments]             TEXT           NULL,
    [RelatedQCIssues]      VARCHAR (100)  NULL,
    [RelatedProcedures]    VARCHAR (1000) NULL,
    CONSTRAINT [PK_NDHP_DAT_Ticket] PRIMARY KEY CLUSTERED ([Ticket] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Previous status of the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'PreviousStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ticket history.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'History';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Free-form text to add to ticket history.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'IssueUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Effective date of current court.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'CourtDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Effective date of current status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'StatusDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current status of the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'ResolutionCause';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of the issue.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Issue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date by which resolution of the issue is required.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Required';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the principal functional area associated with the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Area';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date the ticket was requested.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Requested';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Short description of ticket issue.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Subject';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Three-character ticket type code.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'TicketCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DAT_Ticket', @level2type = N'COLUMN', @level2name = N'Ticket';

