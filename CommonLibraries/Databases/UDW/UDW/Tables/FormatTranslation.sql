CREATE TABLE [dbo].[FormatTranslation] (
    [FmtName] VARCHAR (50)  NOT NULL,
    [Label]   VARCHAR (200) NOT NULL,
    [Start]   VARCHAR (50)  NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [formattranslation]
    ON [dbo].[FormatTranslation]([FmtName] ASC, [Label] ASC, [Start] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Format Name to be used in SAS Format Procedure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FormatTranslation', @level2type = N'COLUMN', @level2name = N'FmtName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Translation for Coded Value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FormatTranslation', @level2type = N'COLUMN', @level2name = N'Label';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code Value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FormatTranslation', @level2type = N'COLUMN', @level2name = N'Start';

