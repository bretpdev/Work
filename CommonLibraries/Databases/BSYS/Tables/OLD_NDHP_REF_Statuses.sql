CREATE TABLE [dbo].[OLD_NDHP_REF_Statuses] (
    [Sequence]  BIGINT       IDENTITY (1, 1) NOT NULL,
    [Ticket]    BIGINT       NOT NULL,
    [Status]    VARCHAR (50) CONSTRAINT [DF_NDHP_REF_Statuses_Status] DEFAULT ('Submitting') NOT NULL,
    [BeginDate] DATETIME     CONSTRAINT [DF_NDHP_REF_Statuses_BeginDate] DEFAULT (getdate()) NOT NULL,
    [EndDate]   DATETIME     CONSTRAINT [DF_NDHP_REF_Statuses_EndDate] DEFAULT ('') NOT NULL,
    [Court]     VARCHAR (50) CONSTRAINT [DF_NDHP_REF_Statuses_Court] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_NDHP_REF_Statuses] PRIMARY KEY CLUSTERED ([Sequence] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the person whose court the ticket is in.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'Court';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'End date of the status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Begin date of the status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'BeginDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status of the ticket.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'Ticket';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique status record sequence number for the table.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Statuses', @level2type = N'COLUMN', @level2name = N'Sequence';

