CREATE TABLE [dbo].[GENR_REF_PriorityCatgryOps] (
    [CatOption]       VARCHAR (200) NOT NULL,
    [Category]        VARCHAR (50)  NOT NULL,
    [DefaultPriority] VARCHAR (50)  NULL,
    CONSTRAINT [PK_GENR_REF_PriorityCatgryOps] PRIMARY KEY CLUSTERED ([CatOption] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Default priority regardless of urgency chosen by user.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GENR_REF_PriorityCatgryOps', @level2type = N'COLUMN', @level2name = N'DefaultPriority';

