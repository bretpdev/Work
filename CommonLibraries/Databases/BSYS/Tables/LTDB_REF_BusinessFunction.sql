CREATE TABLE [dbo].[LTDB_REF_BusinessFunction] (
    [DocName]      NVARCHAR (50)  NOT NULL,
    [BusFunction]  NVARCHAR (100) NOT NULL,
    [PrimFunction] BIT            CONSTRAINT [DF_LTDB_REF_BusinessFunction_PrimFunction] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_LTDB_REF_BusinessFunction] PRIMARY KEY CLUSTERED ([DocName] ASC, [BusFunction] ASC) WITH (FILLFACTOR = 90)
);

