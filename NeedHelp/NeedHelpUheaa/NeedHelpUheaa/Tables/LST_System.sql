CREATE TABLE [dbo].[LST_System] (
    [System]             VARCHAR (100) NOT NULL,
    [ValidForTicketType] CHAR (3)      NOT NULL,
    CONSTRAINT [PK_NDHP_LST_Systems] PRIMARY KEY CLUSTERED ([System] ASC, [ValidForTicketType] ASC) WITH (FILLFACTOR = 90)
);

