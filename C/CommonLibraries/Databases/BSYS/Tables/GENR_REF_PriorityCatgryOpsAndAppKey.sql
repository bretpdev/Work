CREATE TABLE [dbo].[GENR_REF_PriorityCatgryOpsAndAppKey] (
    [CatOption] VARCHAR (200) NOT NULL,
    [AppKey]    VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GENR_REF_PriorityCatgryOpsAndAppKey] PRIMARY KEY CLUSTERED ([AppKey] ASC, [CatOption] ASC),
    CONSTRAINT [FK_GENR_REF_PriorityCatgryOpsAndAppKey_GENR_REF_PriorityCatgryOps] FOREIGN KEY ([CatOption]) REFERENCES [dbo].[GENR_REF_PriorityCatgryOps] ([CatOption]) ON UPDATE CASCADE
);

