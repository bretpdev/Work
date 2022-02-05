CREATE TABLE [dbo].[CallCat_Categories] (
    [Category]     VARCHAR (50) NOT NULL,
    [BusinessUnit] CHAR (1)     NOT NULL,
    CONSTRAINT [PK_CallCat_Categories] PRIMARY KEY CLUSTERED ([Category] ASC) WITH (FILLFACTOR = 90)
);

