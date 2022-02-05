﻿CREATE TABLE [dbo].[RM03_ONL_PAY] (
    [PF_SVC_CLI]         VARCHAR (2)     NOT NULL,
    [PF_IRL_GNR_ID]      VARCHAR (16)    NOT NULL,
    [BF_SSN]             VARCHAR (9)     NOT NULL,
    [BA_PAY]             NUMERIC (9, 2)  NOT NULL,
    [LD_PAY]             DATE            NOT NULL,
    [DF_PAY_ABA]         VARBINARY (300) NULL,
    [DF_BNK_ACC]         VARBINARY (300) NULL,
    [DF_ACC_TYP]         CHAR (1)        NOT NULL,
    [DF_EML]             VARCHAR (128)   NOT NULL,
    [BN_1]               VARCHAR (12)    NOT NULL,
    [BN_MI]              CHAR (1)        NULL,
    [BN_LST]             VARCHAR (35)    NOT NULL,
    [DM_1_ACC_OWN]       VARCHAR (12)    NOT NULL,
    [DM_MI_ACC_OWN]      CHAR (1)        NULL,
    [DM_LST_ACC_OWN]     VARCHAR (35)    NOT NULL,
    [NF_ONL_PAY_DTS]     DATETIME2 (7)   NOT NULL,
    [NF_IPA]             VARCHAR (15)    NOT NULL,
    [NF_IPH]             VARCHAR (75)    NOT NULL,
    [BC_PAY_OPT]         CHAR (1)        NOT NULL,
    [NF_SBM_CPS_DTS]     DATETIME2 (7)   NULL,
    [DF_PAY_ACH]         VARCHAR (9)     NOT NULL,
    [NF_LST_USR_RM03]    VARCHAR (9)     NOT NULL,
    [PC_STA]             VARCHAR (3)     NOT NULL,
    [PD_STA_LST_CHG]     DATE            NOT NULL,
    [PI_BCH_EDT_FAL]     CHAR (1)        NOT NULL,
    [NC_PAY_PRC]         VARCHAR (2)     NOT NULL,
    [PI_PRE_UPL_ACC_INF] CHAR (1)        NOT NULL,
    [LI_NTF_ACC_CHG]     CHAR (1)        NOT NULL,
    [NF_USR_CRT_ONL_PAY] VARCHAR (8)     NOT NULL,
    [LC_NTF_ACC_CHG_REA] VARCHAR (3)     NOT NULL,
    [PI_RQR_CNF_PHN_PAY] CHAR (1)        NOT NULL,
    [PC_OPS_FAT_TYP]     VARCHAR (2)     NOT NULL,
    [PC_OPS_FAT_SUB_TYP] VARCHAR (2)     NOT NULL,
    [DF_OPS_PRS_PAY_SSN] VARCHAR (9)     NOT NULL,
    [DC_OPS_PRS_PAY_TYP] CHAR (1)        NOT NULL,
    CONSTRAINT [PK_RM03_ONL_PAY] PRIMARY KEY CLUSTERED ([PF_SVC_CLI] ASC, [PF_IRL_GNR_ID] ASC, [BF_SSN] ASC) WITH (FILLFACTOR = 95)
);


GO
CREATE NONCLUSTERED INDEX [IX_RM03_LD_PAY_BA_PAY]
    ON [dbo].[RM03_ONL_PAY]([LD_PAY] ASC)
    INCLUDE([BA_PAY]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
