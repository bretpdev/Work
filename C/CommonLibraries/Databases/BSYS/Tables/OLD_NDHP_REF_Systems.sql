CREATE TABLE [dbo].[OLD_NDHP_REF_Systems] (
    [Ticket] BIGINT        NOT NULL,
    [System] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NDHP_REF_Systems] PRIMARY KEY CLUSTERED ([Ticket] ASC, [System] ASC)
);

