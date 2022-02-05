CREATE TABLE [dbo].[LST_FacilitiesPriorityCatgry] (
    [CategoryOption] VARCHAR (100) NOT NULL,
    [Category]       VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_NDHP_LST_FacilitiesPriorityCatgrys] PRIMARY KEY CLUSTERED ([CategoryOption] ASC) WITH (FILLFACTOR = 90)
);

