﻿CREATE TABLE [dbo].[ZDEL_RS05_IDR_Repayment]
(
	[DF_SPE_ACC_ID] VARCHAR(10) NOT NULL , 
	[BD_CRT_RS05] DATETIME NOT NULL, 
    [BN_IBR_SEQ] INT NOT NULL, 
    PRIMARY KEY ([DF_SPE_ACC_ID], [BD_CRT_RS05], [BN_IBR_SEQ])
)