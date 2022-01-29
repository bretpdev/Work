CREATE TABLE [dbo].[SCKR_REF_BusinessFunctionSAS] (
    [Program]      NVARCHAR (100) NOT NULL,
    [BusFunction]  NVARCHAR (100) NOT NULL,
    [PrimFunction] BIT            CONSTRAINT [DF_SCKR_REF_BusinessFunctionSAS_PrimFunction] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_refBusinessFunctionSAS] PRIMARY KEY CLUSTERED ([Program] ASC, [BusFunction] ASC) WITH (FILLFACTOR = 90)
);

