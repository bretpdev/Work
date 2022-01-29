CREATE TABLE [dbo].[REF_System] (
    [Ticket] BIGINT        NOT NULL,
    [System] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NDHP_REF_Systems] PRIMARY KEY CLUSTERED ([Ticket] ASC, [System] ASC) WITH (FILLFACTOR = 90)
);

