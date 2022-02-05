CREATE TABLE [dbo].[LN15_Disbursement] (
    [DF_SPE_ACC_ID] VARCHAR (10)   NOT NULL,
    [LN_BR_DSB_SEQ] INT            NOT NULL,
    [LA_DSB]        NUMERIC (9, 2) NULL,
    [LD_DSB]        VARCHAR (10)   NULL,
    [LC_DSB_TYP]    VARCHAR (1)    NULL,
    [LC_STA_LON15]  VARCHAR (1)    NULL,
    [LN_SEQ]        INT            NULL,
    [LA_DL_REBATE]  NUMERIC (12)   NULL,
    PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_BR_DSB_SEQ] ASC)
);

