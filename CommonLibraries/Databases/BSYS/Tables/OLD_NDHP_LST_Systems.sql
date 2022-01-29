CREATE TABLE [dbo].[OLD_NDHP_LST_Systems] (
    [System]             VARCHAR (100) NOT NULL,
    [ValidForTicketType] CHAR (3)      NOT NULL,
    CONSTRAINT [PK_NDHP_LST_Systems] PRIMARY KEY CLUSTERED ([System] ASC, [ValidForTicketType] ASC)
);

