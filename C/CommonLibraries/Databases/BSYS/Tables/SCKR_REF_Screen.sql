﻿CREATE TABLE [dbo].[SCKR_REF_Screen] (
    [Script] NVARCHAR (50) NOT NULL,
    [Screen] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_refScreen] PRIMARY KEY CLUSTERED ([Script] ASC, [Screen] ASC) WITH (FILLFACTOR = 90)
);

