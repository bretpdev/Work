﻿CREATE TABLE [dbo].[LTDB_LST_Days2Long] (
    [Day2tooOld] INT NOT NULL,
    CONSTRAINT [PK_LTDB_LST_Days2Long] PRIMARY KEY CLUSTERED ([Day2tooOld] ASC) WITH (FILLFACTOR = 90)
);
