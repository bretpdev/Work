CREATE TABLE [dbo].[SCKR_REF_BusinessFunction] (
    [Program]      NVARCHAR (100) NOT NULL,
    [BusFunction]  NVARCHAR (100) NOT NULL,
    [PrimFunction] BIT            CONSTRAINT [DF_SCKR_REF_BusinessFunction_PrimFunction] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_refBusinessFunction] PRIMARY KEY CLUSTERED ([Program] ASC, [BusFunction] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SCKR_REF_BusinessFunction_SCKR_DAT_Scripts] FOREIGN KEY ([Program]) REFERENCES [dbo].[SCKR_DAT_Scripts] ([Script]) ON UPDATE CASCADE
);

