﻿CREATE TABLE [dbo].[LN40_LON_CLM_PCL] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LN_SEQ_CLM_PCL]     SMALLINT        NOT NULL,
    [LA_SBM_CLM_PCL_PRI] NUMERIC (8, 2)  NOT NULL,
    [LA_SBM_CLM_PCL_INT] NUMERIC (7, 2)  NOT NULL,
    [LI_CLM_PKG_RTN_RCV] CHAR (1)        NOT NULL,
    [LC_CLM_REJ_RTN_LIB] VARCHAR (2)     NOT NULL,
    [LI_GTR_ACK_CLM_CAN] CHAR (1)        NOT NULL,
    [LC_REA_CAN_CLM_PCL] VARCHAR (2)     NOT NULL,
    [LD_CRT_CLM_PCL]     DATE            NOT NULL,
    [LD_CLM_REJ_RTN_ACL] DATE            NULL,
    [LD_CLM_REJ_RTN_EFF] DATE            NULL,
    [LD_CLM_REJ_RTN_MAX] DATE            NULL,
    [LD_SBM_CLM_PCL]     DATE            NULL,
    [LD_CAN_CLM_PCL]     DATE            NULL,
    [LC_TYP_REJ_RTN]     CHAR (1)        NOT NULL,
    [LF_LST_DTS_LN40]    DATETIME2 (7)   NOT NULL,
    [LC_TYP_REC_CLP_LON] CHAR (1)        NOT NULL,
    [LC_REA_CLP_LON]     VARCHAR (2)     NOT NULL,
    [LI_TSK_CRT_RSI_BAL] CHAR (1)        NOT NULL,
    [LN_SEQ_CLM_PCL_ORG] SMALLINT        NULL,
    [LD_CND_OCC]         DATE            NULL,
    [LD_CLM_PD_PCV]      DATE            NULL,
    [LA_CLM_PD_PCV]      NUMERIC (8, 2)  NULL,
    [LD_CLM_ORG_CRT]     DATE            NULL,
    [LD_CLM_ORG_SBM]     DATE            NULL,
    [LI_PCL_CLM_PCV]     CHAR (1)        NOT NULL,
    [LC_REA_CLM_REJ_RTN] VARCHAR (2)     NOT NULL,
    [LC_SUP_PCA]         CHAR (1)        NOT NULL,
    [LD_OSD_CLM]         DATE            NULL,
    [LD_NTF_OSD_CLM]     DATE            NULL,
    [LI_RPD_CHG_CLM]     CHAR (1)        NOT NULL,
    [LD_1_PAY_DU_CLM]    DATE            NULL,
    [LA_TOT_BR_PAY_CLM]  NUMERIC (8, 2)  NULL,
    [LN_MTH_PAY_CLM]     NUMERIC (3)     NULL,
    [LN_MTH_DFR_CLM]     NUMERIC (3)     NULL,
    [LN_MTH_FOR_CLM]     NUMERIC (3)     NULL,
    [LN_MTH_VIO_CLM]     NUMERIC (3)     NULL,
    [LN_DFR_FOR_EVT_CLM] NUMERIC (3)     NULL,
    [LN_MTH_RNV_CLM]     NUMERIC (3)     NULL,
    [LD_PAY_DU_CLM]      DATE            NULL,
    [LA_TOT_DSB_CLM]     NUMERIC (8, 2)  NULL,
    [LA_CAP_INT_CLM]     NUMERIC (8, 2)  NULL,
    [LA_PRI_RPD_CLM]     NUMERIC (8, 2)  NULL,
    [LA_CU_INT_CAP_CLM]  NUMERIC (8, 2)  NULL,
    [LD_INT_PD_THU_CLM]  DATE            NULL,
    [LD_CLM_INT_CLM]     DATE            NULL,
    [LA_UNP_INT_NO_CAP]  NUMERIC (8, 2)  NULL,
    [LD_CLM_REJ_LTR]     DATE            NULL,
    [LA_DSA_RFD_CLM]     NUMERIC (8, 2)  NULL,
    [LD_CLM_PD_LTR]      DATE            NULL,
    [LN_CCI_CLM_SEQ]     SMALLINT        NULL,
    [LD_CCI_LON_SLD]     DATE            NULL,
    [LD_CCI_SER_RSB]     DATE            NULL,
    [LD_XCP_PRF]         DATE            NULL,
    [LA_CCI_UNP_FEE]     NUMERIC (9, 2)  NULL,
    [LA_CCI_UNP_INT]     NUMERIC (9, 2)  NULL,
    [LA_ITL_STD_PAY_CLM] NUMERIC (12, 2) NULL,
    [LA_PMN_STD_PAY_CLM] NUMERIC (12, 2) NULL,
    [LD_25_YR_FGV_CLM]   DATE            NULL,
    [LN_MTH_QLF_FGV_CLM] NUMERIC (3)     NULL,
    [LD_IBR_SR_CLM]      DATE            NULL,
    [LN_DAY_EHD_DFR_CLM] NUMERIC (5)     NULL,
    CONSTRAINT [PK_LN40_LON_CLM_PCL] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_SEQ_CLM_PCL] ASC)
);
