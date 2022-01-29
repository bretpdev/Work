CREATE TABLE [dbo].[GENR_LST_States] (
    [Code]        CHAR (2)     NOT NULL,
    [shortDesc]   VARCHAR (50) NULL,
    [Description] VARCHAR (50) NULL,
    [IsDomestic]  BIT          CONSTRAINT [DF_GENR_LST_States_IsDomestic] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GENR_LST_States] PRIMARY KEY CLUSTERED ([Code] ASC)
);

