﻿CREATE TABLE [dbo].[PD20_PRS_DTH]
(
[DF_PRS_ID] VARCHAR(9) NOT NULL, 
[DD_DTH_NTF] DATETIME NOT NULL, 
[DD_DTH] DATETIME NULL, 
[DM_DTH_CT] VARCHAR(20) NULL, 
[DM_DTH_CTY] VARCHAR(15) NULL, 
[DC_DTH_ST] VARCHAR(2) NULL, 
[DF_SUR_PRS_ID] VARCHAR(9) NULL, 
[DM_DTH_FGN_CNY] VARCHAR(15) NULL, 
[PF_LST_DTS_PD20] DATETIME NULL, 
[IF_IST] VARCHAR(8) NULL, 

    PRIMARY KEY ([DF_PRS_ID], 
				 [DD_DTH_NTF]
				)
)
