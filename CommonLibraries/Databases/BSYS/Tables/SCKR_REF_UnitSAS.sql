﻿CREATE TABLE [dbo].[SCKR_REF_UnitSAS] (
    [Program] NVARCHAR (100) NOT NULL,
    [Unit]    NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_refUnitSAS] PRIMARY KEY CLUSTERED ([Program] ASC, [Unit] ASC) WITH (FILLFACTOR = 90)
);

