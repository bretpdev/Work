CREATE TABLE [Staging].[Deferment] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NULL,
    [LN_SEQ]             INT            NULL,
    [LF_DFR_CTL_NUM]     VARCHAR (3)    NULL,
    [LN_DFR_OCC_SEQ]     INT            NULL,
    [LC_DFR_TYP]         VARCHAR (2)    NULL,
    [LD_DFR_INF_CER]     VARCHAR (10)   NULL,
    [LD_DFR_BEG]         VARCHAR (10)   NULL,
    [LD_DFR_END]         VARCHAR (10)   NULL,
    [LC_LON_LEV_DFR_CAP] VARCHAR (1)    NULL,
    [LC_STA_LON50]       VARCHAR (1)    NULL,
    [LC_DFR_STA]         VARCHAR (1)    NULL,
    [LC_STA_DFR10]       VARCHAR (1)    NULL,
    [DFR_TYP]            VARCHAR (200)  NULL,
    [MONTHS_USED]        DECIMAL (7, 1) NULL
);

