CREATE TABLE [dbo].[LST_FacilitiesPriority] (
    [Category] VARCHAR (50) NOT NULL,
    [Urgency]  VARCHAR (50) NOT NULL,
    [Priority] SMALLINT     NOT NULL,
    CONSTRAINT [PK_NDHP_LST_FacilitiesPriorities] PRIMARY KEY CLUSTERED ([Category] ASC, [Urgency] ASC) WITH (FILLFACTOR = 90)
);

