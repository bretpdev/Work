CREATE TABLE [dbo].[OLD_NDHP_LST_FacilitiesPriorities] (
    [Category] VARCHAR (50) NOT NULL,
    [Urgency]  VARCHAR (50) NOT NULL,
    [Priority] SMALLINT     NOT NULL,
    CONSTRAINT [PK_NDHP_LST_FacilitiesPriorities_1] PRIMARY KEY CLUSTERED ([Category] ASC, [Urgency] ASC)
);

