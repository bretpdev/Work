﻿CREATE TABLE [dbo].[ZDEL_AD20_FinActAdjustment]
(
	[DF_SPE_ACC_ID] VARCHAR(10) NOT NULL , 
    [LD_FAT_ADJ_REQ] VARCHAR(10) NOT NULL, 
    [LN_SEQ_FAT_ADJ_REQ] INT NOT NULL, 
    PRIMARY KEY ([DF_SPE_ACC_ID], [LD_FAT_ADJ_REQ], [LN_SEQ_FAT_ADJ_REQ])
)