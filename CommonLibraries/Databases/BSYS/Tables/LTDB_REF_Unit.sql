﻿CREATE TABLE [dbo].[LTDB_REF_Unit] (
    [DocName] NVARCHAR (50) NOT NULL,
    [Unit]    NVARCHAR (50) NOT NULL,
    [Manager] NVARCHAR (50) NULL,
    CONSTRAINT [PK_LTDB_REF_Unit] PRIMARY KEY CLUSTERED ([DocName] ASC, [Unit] ASC) WITH (FILLFACTOR = 90)
);

