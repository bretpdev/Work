CREATE TABLE [dbo].[SCKR_REF_EMail] (
    [Sequence]  INT           IDENTITY (1, 1) NOT NULL,
    [Request]   INT           NULL,
    [Class]     NVARCHAR (3)  NULL,
    [Recipient] NVARCHAR (50) NULL,
    CONSTRAINT [PK_refEMail] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

