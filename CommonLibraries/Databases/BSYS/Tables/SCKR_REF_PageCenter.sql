CREATE TABLE [dbo].[SCKR_REF_PageCenter] (
    [SeqNo]       INT            IDENTITY (1, 1) NOT NULL,
    [Job]         NVARCHAR (100) NULL,
    [ReportNo]    NVARCHAR (3)   NULL,
    [ShortDesc]   NVARCHAR (30)  NULL,
    [Mailbox]     NVARCHAR (50)  NULL,
    [Application] NVARCHAR (50)  NULL,
    [Recipients]  NVARCHAR (100) NULL,
    [PrintOpt]    NVARCHAR (2)   NULL,
    CONSTRAINT [PK_refPageCenter] PRIMARY KEY CLUSTERED ([SeqNo] ASC) WITH (FILLFACTOR = 90)
);

