CREATE TABLE [dbo].[ServicerCategory] (
    [ServicerCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [ServicerCategory]   VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([ServicerCategoryId] ASC)
);

