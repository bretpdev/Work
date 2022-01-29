CREATE TABLE [dbo].[DGBH_DAT_Tasks] (
    [Number]   INT           NOT NULL,
    [Title]    VARCHAR (100) NULL,
    [Priority] FLOAT (53)    NULL,
    [TopTen]   BIT           NULL,
    [Summary]  VARCHAR (500) NULL,
    [Court]    VARCHAR (50)  NULL,
    [Status]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_DGBH_DAT_Tasks] PRIMARY KEY CLUSTERED ([Number] ASC)
);

