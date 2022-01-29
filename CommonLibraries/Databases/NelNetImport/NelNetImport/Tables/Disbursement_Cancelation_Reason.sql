CREATE TABLE [dbo].[Disbursement_Cancelation_Reason] (
    [br_ssn]             VARCHAR (9) NULL,
    [ln_num]             VARCHAR (2) NULL,
    [db_disb_number]     VARCHAR (2) NULL,
    [da_reason_code]     VARCHAR (2) NULL,
    [db_seq]             VARCHAR (2) NULL,
    [aes_reason_code]    CHAR (4)    NULL,
    [BF_SSN]             VARCHAR (9) NULL,
    [LN_BR_DSB_SEQ_temp] VARCHAR (2) NULL,
    [LC_DSB_CAN_REA]     VARCHAR (4) NULL,
    [LC_DSB_FEE]         CHAR (7)    NULL,
    [LA_DSB_CAN]         MONEY       NULL,
    [LD_DSB_CAN]         CHAR (7)    NULL,
    [LD_CAN_REP_GTR]     CHAR (7)    NULL,
    [LA_DSB_CAN_RFD]     MONEY       NULL,
    [LD_DSB_CAN_RFD]     CHAR (7)    NULL,
    [LA_FEE_CAN]         MONEY       NULL,
    [LD_FEE_CAN]         CHAR (7)    NULL,
    [LA_PRE_DSB_CAN]     MONEY       NULL,
    [LD_PRE_DSB_CAN]     CHAR (7)    NULL,
    [LD_DSB]             CHAR (7)    NULL
);

