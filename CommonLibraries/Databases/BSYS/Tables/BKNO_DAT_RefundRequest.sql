CREATE TABLE [dbo].[BKNO_DAT_RefundRequest] (
    [Index]        INT          IDENTITY (1, 1) NOT NULL,
    [RecordNumber] VARCHAR (50) NULL,
    [Name]         VARCHAR (50) NULL,
    [SSN]          VARCHAR (10) NULL,
    [Address1]     VARCHAR (35) NULL,
    [Address2]     VARCHAR (35) NULL,
    [City]         VARCHAR (30) NULL,
    [State]        CHAR (2)     NULL,
    [Zip]          VARCHAR (10) NULL,
    [Refund]       VARCHAR (10) NULL,
    [TranType]     CHAR (2)     NULL,
    [DateApplied]  CHAR (10)    NULL,
    [Amount]       VARCHAR (10) NULL,
    CONSTRAINT [PK_BKNO_DAT_RefundRequest] PRIMARY KEY CLUSTERED ([Index] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BKNO_DAT_RefundRequest]
    ON [dbo].[BKNO_DAT_RefundRequest]([Index] ASC);

