﻿CREATE TABLE [dbo].[ClosedSchoolFSA]
(
	[ClosedSchoolFSAId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [BF_SSN] CHAR(9) NOT NULL, 
    [PF_REQ_ACT] VARCHAR(5) NOT NULL, 
    [LD_ATY_REQ_RCV] DATETIME NOT NULL
)
