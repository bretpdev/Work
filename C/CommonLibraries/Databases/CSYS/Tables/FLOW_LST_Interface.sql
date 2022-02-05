CREATE TABLE [dbo].[FLOW_LST_Interface] (
    [Interface] VARCHAR (10) NOT NULL,
    [System]    VARCHAR (30) NULL,
    CONSTRAINT [PK_FLOW_LST_Interface_1] PRIMARY KEY CLUSTERED ([Interface] ASC),
    CONSTRAINT [FK_FLOW_LST_Interface_GENR_LST_System] FOREIGN KEY ([System]) REFERENCES [dbo].[GENR_LST_System] ([System]),
    CONSTRAINT [uc_Interface] UNIQUE NONCLUSTERED ([Interface] ASC, [System] ASC)
);

