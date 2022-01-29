﻿CREATE TABLE [dbo].[ZDEL_RP02_BENEFITPROGRAMTIERS]
(
	[DF_SPE_ACC_ID] VARCHAR(10) NOT NULL, 
	[PM_BBS_PGM] VARCHAR(3) NOT NULL , 
    [PN_BBS_PGM_SEQ] INT NOT NULL, 
    [PF_BBS_PGM_TIR] VARCHAR(2) NOT NULL, 
    [PN_BBS_PGM_TIR_SEQ] INT NOT NULL, 
    CONSTRAINT [PK_ZDEL_RP02_BENEFITPROGRAMTIERS] PRIMARY KEY ([DF_SPE_ACC_ID], [PM_BBS_PGM], [PN_BBS_PGM_SEQ], [PF_BBS_PGM_TIR], [PN_BBS_PGM_TIR_SEQ])
)
