﻿CREATE TABLE [dbo].[Aging360AtServicer]
(
	[Aging360AtServicerId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [BF_SSN] CHAR(9) NOT NULL, 
    [LN_SEQ] SMALLINT NOT NULL, 
    [AGING_DATE] DATETIME NOT NULL
)