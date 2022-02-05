CREATE TABLE [dbo].[FWDF_DAT_Lenders] (
    [SASFile]     VARCHAR (50)  NOT NULL,
    [LenderName]  VARCHAR (100) NOT NULL,
    [LenderEmail] VARCHAR (50)  NULL,
    CONSTRAINT [PK_FWDF_DAT_LenderData] PRIMARY KEY CLUSTERED ([SASFile] ASC)
);

