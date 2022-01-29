CREATE TABLE [fp].[EcorrCategories] (
    [EcorrCategoriesId] INT          IDENTITY (1, 1) NOT NULL,
    [ScriptFileId]      INT          NOT NULL,
    [EcorrFieldName]    VARCHAR (25) NOT NULL,
    PRIMARY KEY CLUSTERED ([EcorrCategoriesId] ASC) WITH (FILLFACTOR = 95)
);

