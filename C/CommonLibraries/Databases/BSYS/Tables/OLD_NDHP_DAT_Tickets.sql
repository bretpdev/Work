CREATE TABLE [dbo].[OLD_NDHP_DAT_Tickets] (
    [Ticket]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [TicketCode]           CHAR (3)      NOT NULL,
    [Subject]              VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_Subject] DEFAULT ('') NULL,
    [TestRequester]        NVARCHAR (50) CONSTRAINT [DF_NDHP_DAT_Tickets_Requester] DEFAULT ('') NULL,
    [Requested]            DATETIME      CONSTRAINT [DF_NDHP_DAT_Tickets_Requested] DEFAULT (getdate()) NULL,
    [Unit]                 VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_Unit] DEFAULT ('') NULL,
    [Area]                 VARCHAR (100) CONSTRAINT [DF_NDHP_DAT_Tickets_Area] DEFAULT ('') NULL,
    [Required]             DATETIME      NULL,
    [Issue]                TEXT          CONSTRAINT [DF_NDHP_DAT_Tickets_Issue] DEFAULT ('') NULL,
    [ResolutionCause]      VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_Resolution] DEFAULT ('') NULL,
    [ResolutionFix]        TEXT          CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionFix] DEFAULT ('') NULL,
    [ResolutionPrevention] TEXT          CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionPreventiion] DEFAULT ('') NULL,
    [Status]               VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_Status] DEFAULT ('Submitting') NULL,
    [StatusDate]           DATETIME      CONSTRAINT [DF_NDHP_DAT_Tickets_StatusDate] DEFAULT (getdate()) NULL,
    [TestCourt]            NVARCHAR (50) CONSTRAINT [DF_NDHP_DAT_Tickets_Court] DEFAULT ('') NULL,
    [CourtDate]            DATETIME      CONSTRAINT [DF_NDHP_DAT_Tickets_CourtDate] DEFAULT (getdate()) NULL,
    [IssueUpdate]          TEXT          CONSTRAINT [DF_NDHP_DAT_Tickets_IssueUpdate] DEFAULT ('') NULL,
    [History]              TEXT          CONSTRAINT [DF_NDHP_DAT_Tickets_History] DEFAULT ('') NULL,
    [PreviousStatus]       VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_PreviousStatus] DEFAULT ('') NULL,
    [TestPreviousCourt]    VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_PreviousCourt] DEFAULT ('') NULL,
    [UrgencyOption]        VARCHAR (200) CONSTRAINT [DF_NDHP_DAT_Tickets_Urgency] DEFAULT ('') NULL,
    [CatOption]            VARCHAR (200) CONSTRAINT [DF_NDHP_DAT_Tickets_Cat] DEFAULT ('') NULL,
    [Priority]             SMALLINT      NULL,
    [LastUpdated]          DATETIME      CONSTRAINT [DF_NDHP_DAT_Tickets_LastUpdated] DEFAULT (getdate()) NULL,
    [CCCIssue]             VARCHAR (20)  CONSTRAINT [DF_NDHP_DAT_Tickets_CCCIssue] DEFAULT ('') NULL,
    [RequestProjectNum]    VARCHAR (50)  CONSTRAINT [DF_NDHP_DAT_Tickets_RequestProjectNum] DEFAULT ('') NULL,
    [TestAssignedTo]       NVARCHAR (50) CONSTRAINT [DF_NDHP_DAT_Tickets_AssignedTo] DEFAULT ('') NULL,
    [Comments]             TEXT          NULL,
    CONSTRAINT [PK_NDHP_DAT_Tickets] PRIMARY KEY CLUSTERED ([Ticket] ASC),
    CONSTRAINT [FK_NDHP_DAT_Tickets_NDHP_LST_TicketTypes] FOREIGN KEY ([TicketCode]) REFERENCES [dbo].[OLD_NDHP_LST_TicketTypes] ([TicketCode]) ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the person whose court the ticket was in previously.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'TestPreviousCourt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Previous status of the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'PreviousStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ticket history.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'History';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Free-form text to add to ticket history.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'IssueUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Effective date of current court.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'CourtDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the person whose court the ticket is in.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'TestCourt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Effective date of current status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'StatusDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current status of the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'ResolutionCause';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of the issue.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Issue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date by which resolution of the issue is required.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Required';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the principal functional area associated with the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Area';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the principal business unit associate to the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Unit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date the ticket was requested.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Requested';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of ticket requester.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'TestRequester';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Short description of ticket issue.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Subject';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Three-character ticket type code.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'TicketCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_DAT_Tickets', @level2type = N'COLUMN', @level2name = N'Ticket';

