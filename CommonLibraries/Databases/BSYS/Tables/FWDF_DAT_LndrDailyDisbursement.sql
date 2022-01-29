CREATE TABLE [dbo].[FWDF_DAT_LndrDailyDisbursement] (
    [SASFile]     VARCHAR (50) NOT NULL,
    [LenderName]  VARCHAR (50) NOT NULL,
    [LenderEmail] VARCHAR (50) NULL,
    CONSTRAINT [PK_FWDF_DAT_LndrDailyDisbursement] PRIMARY KEY CLUSTERED ([SASFile] ASC)
);

