CREATE TABLE [dbo].[DCR_U_Repayment_LN65] (
    [BF_SSN]             VARCHAR (9)     NULL,
    [LN_SEQ]             INT             NULL,
    [LN_RPS_SEQ_temp]    INT             NOT NULL,
    [LA_CPI_RPD_DIS]     MONEY           NULL,
    [LR_INT_RPD_DIS]     DECIMAL (10, 3) NULL,
    [LC_STA_LON65]       VARCHAR (1)     NULL,
    [LC_TYP_SCH_DIS]     VARCHAR (2)     NULL,
    [LA_ACR_INT_RPD]     INT             NULL,
    [LN_RPD_MAX_TRM_REQ] VARCHAR (3)     NULL,
    [LD_RPD_MAX_TRM_SR]  DATE            NULL,
    [LC_RPD_DIS_temp]    INT             NULL
);

