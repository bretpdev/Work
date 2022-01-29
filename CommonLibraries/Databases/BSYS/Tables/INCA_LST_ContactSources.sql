CREATE TABLE [dbo].[INCA_LST_ContactSources] (
    [Source]       VARCHAR (50) NOT NULL,
    [ActivityType] CHAR (2)     NOT NULL,
    [ContactType]  CHAR (2)     NOT NULL,
    CONSTRAINT [PK_INCA_LST_ContactSources] PRIMARY KEY CLUSTERED ([Source] ASC)
);

