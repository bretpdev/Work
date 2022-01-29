CREATE TABLE [dbo].[LN66_LON_RPS_SPF] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LN_RPS_SEQ]         SMALLINT        NOT NULL,
    [LN_GRD_RPS_SEQ]     SMALLINT        NOT NULL,
    [LA_RPS_ISL]         NUMERIC (7, 2)  NULL,
    [LD_CRT_LON66]       DATE            NOT NULL,
    [LN_RPS_TRM]         NUMERIC (3)     NOT NULL,
    [LF_LST_DTS_LN66]    DATETIME2 (7)   NOT NULL,
    [LA_PRI_RDC_GRD]     NUMERIC (8, 2)  NULL,
    [LN_PRI_RDC_GRD_TRM] NUMERIC (3)     NULL,
    [LA_PRI_ATU_PAY]     NUMERIC (12, 2) NULL,
    [LD_RPYE_FGV]        DATE            NULL,
    [LA_RPYE_CTH_ISL]    NUMERIC (12, 2) NULL
);


