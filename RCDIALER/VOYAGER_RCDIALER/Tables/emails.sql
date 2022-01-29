CREATE TABLE [dbo].[emails] (
    [borrower_id]          INT           NOT NULL,
    [school_id]            VARCHAR (6)   NOT NULL,
    [email_source]         VARCHAR (50)  NULL,
    [email_effective_date] DATE          NULL,
    [email_good_flag]      CHAR (1)      NULL,
    [email_address]        VARCHAR (255) NOT NULL,
    [created_on]           DATETIME      CONSTRAINT [DF_emails_created_on] DEFAULT (getdate()) NOT NULL,
    [modified_on]          DATETIME      CONSTRAINT [DF_emails_modified_on] DEFAULT (getdate()) NOT NULL
);

