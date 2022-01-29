CREATE TABLE [dbo].[BKNO_DAT_ChangeOfAddress] (
    [Index]     INT          IDENTITY (1, 1) NOT NULL,
    [DocketNum] CHAR (12)    NULL,
    [SSN]       VARCHAR (10) NULL,
    [Name]      VARCHAR (50) NULL,
    CONSTRAINT [PK_BKNO_DAT_ChangeOfAddress] PRIMARY KEY CLUSTERED ([Index] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BKNO_DAT_ChangeOfAddress]
    ON [dbo].[BKNO_DAT_ChangeOfAddress]([Index] ASC);

