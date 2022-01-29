CREATE TABLE [dbo].[LST_IncidentPriority] (
    [Priority]     VARCHAR (10) NOT NULL,
    [NumericValue] SMALLINT     CONSTRAINT [DF_LST_IncidentPriority_NumericValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LST_IncidentPriority] PRIMARY KEY CLUSTERED ([Priority] ASC)
);

