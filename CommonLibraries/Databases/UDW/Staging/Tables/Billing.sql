CREATE TABLE [Staging].[Billing] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NULL,
    [LN_SEQ]             INT            NULL,
    [LD_BIL_CRT]         VARCHAR (10)   NULL,
    [LN_SEQ_BIL_WI_DTE]  INT            NULL,
    [LD_BIL_DU_LON]      VARCHAR (10)   NULL,
    [LC_STA_LON80]       VARCHAR (1)    NULL,
    [LA_BIL_CUR_DU]      DECIMAL (8, 2) NULL,
    [LA_BIL_PAS_DU]      DECIMAL (8, 2) NULL,
    [LC_BIL_MTD]         VARCHAR (1)    NULL,
    [LC_IND_BIL_SNT]     VARCHAR (1)    NULL,
    [LC_STA_BIL10]       VARCHAR (1)    NULL,
    [LA_TOT_BIL_STS]     DECIMAL (8, 2) NULL,
    [LD_BIL_STS_RIR_TOL] VARCHAR (10)   NULL,
    [PAID_AHEAD]         VARCHAR (1)    NULL,
    [BIL_SAT]            VARCHAR (1)    NULL,
    [BIL_MTD]            VARCHAR (200)  NULL
);

