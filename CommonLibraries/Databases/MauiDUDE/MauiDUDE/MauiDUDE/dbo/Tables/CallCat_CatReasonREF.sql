CREATE TABLE [dbo].[CallCat_CatReasonREF] (
    [Category]     VARCHAR (50) NOT NULL,
    [Reason]       VARCHAR (50) NOT NULL,
    [BusinessUnit] CHAR (1)     NOT NULL,
    CONSTRAINT [PK_CallCat_CatReasonREF] PRIMARY KEY CLUSTERED ([Category] ASC, [Reason] ASC) WITH (FILLFACTOR = 90)
);

