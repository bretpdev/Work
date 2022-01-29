CREATE TABLE [dbo].[SCKR_REF_BSPASign] (
    [Request]    INT           NULL,
    [Class]      NVARCHAR (3)  NULL,
    [Unit]       NVARCHAR (50) NULL,
    [Signed]     BIT           CONSTRAINT [DF_SCKR_REF_BSPASign_Signed] DEFAULT (0) NOT NULL,
    [Agent]      NVARCHAR (50) NULL,
    [SignedDate] SMALLDATETIME NULL
);

