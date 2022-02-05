CREATE TABLE [dbo].[GENR_LST_States] (
    [Code]        CHAR (2)     NOT NULL,
    [shortDesc]   VARCHAR (50) NULL,
    [Description] VARCHAR (50) NULL,
    [Domestic]    CHAR (1)     NULL,
    CONSTRAINT [PK_GENR_LST_States] PRIMARY KEY CLUSTERED ([Code] ASC)
);

