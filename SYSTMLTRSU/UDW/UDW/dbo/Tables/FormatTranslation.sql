﻿CREATE TABLE [dbo].[FormatTranslation] (
    [FmtName] VARCHAR (50)  NOT NULL,
    [Label]   VARCHAR (200) NOT NULL,
    [Start]   VARCHAR (50)  NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [formattranslation]
    ON [dbo].[FormatTranslation]([FmtName] ASC, [Label] ASC, [Start] ASC);
