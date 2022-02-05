CREATE TABLE [dbo].[SCKR_REF_SASScript] (
    [Sequence] INT            IDENTITY (1, 1) NOT NULL,
    [Script]   NVARCHAR (100) NULL,
    [Job]      NVARCHAR (100) NULL,
    CONSTRAINT [PK_refSASScript] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

