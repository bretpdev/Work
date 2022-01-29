﻿CREATE TABLE [dbo].[SCKR_LST_AutoRecips] (
    [Agent] NVARCHAR (50) NOT NULL,
    [Class] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_lstAutoRecips] PRIMARY KEY CLUSTERED ([Agent] ASC, [Class] ASC) WITH (FILLFACTOR = 90)
);

