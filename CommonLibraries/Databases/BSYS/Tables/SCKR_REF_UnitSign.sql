CREATE TABLE [dbo].[SCKR_REF_UnitSign] (
    [Request]    INT           NOT NULL,
    [Class]      NVARCHAR (3)  NOT NULL,
    [Unit]       NVARCHAR (50) NOT NULL,
    [Signed]     BIT           CONSTRAINT [DF_SCKR_REF_UnitSign_Signed] DEFAULT (0) NOT NULL,
    [Agent]      NVARCHAR (50) NULL,
    [SignedDate] SMALLDATETIME NULL,
    CONSTRAINT [PK_refUnitSign] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [Unit] ASC) WITH (FILLFACTOR = 90)
);

