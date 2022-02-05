CREATE TABLE [dbo].[GENR_LST_BusinessUnits] (
    [ID]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [Status] VARCHAR (10) NULL,
    CONSTRAINT [PK__GENR_LST_BusinessUnits_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 95)
);



