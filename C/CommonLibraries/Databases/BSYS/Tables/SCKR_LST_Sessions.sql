﻿CREATE TABLE [dbo].[SCKR_LST_Sessions] (
    [Session] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_lstSessions] PRIMARY KEY CLUSTERED ([Session] ASC) WITH (FILLFACTOR = 90)
);
