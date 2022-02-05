CREATE TABLE [calc].[RepaymentSchedules] (
    [BF_SSN]             CHAR (9)        NULL,
    [LN_SEQ]             INT             NULL,
    [LN_RPS_SEQ]         INT             NULL,
    [LN_GRD_RPS_SEQ]     INT             NULL,
    [TermStartDate]      DATE            NULL,
    [IC_LON_PGM]         VARCHAR (6)     NULL,
    [LD_LON_1_DSB]       DATE            NULL,
    [LA_LON_AMT_GTR]     DECIMAL (18, 2) NULL,
    [LA_CUR_PRI]         DECIMAL (18, 2) NULL,
    [LR_ITR]             DECIMAL (5, 3)  NULL,
    [LC_TYP_SCH_DIS]     VARCHAR (2)     NULL,
    [LA_TOT_RPD_DIS]     DECIMAL (18, 2) NULL,
    [LA_ANT_CAP]         DECIMAL (18, 2) NULL,
    [LD_RPS_1_PAY_DU]    DATE            NULL,
    [LN_RPS_TRM]         INT             NULL,
    [LA_RPS_ISL]         DECIMAL (18, 2) NULL,
    [TotalDfrMonthsUsed] INT             NULL,
    [TotalFrbMonthsUsed] INT             NULL,
    [GradationMonths]    INT             NULL,
    [TermsToDate]        INT             NULL,
    [CurrentGradation]   BIT             NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_CurrentGradation]
    ON [calc].[RepaymentSchedules]([CurrentGradation] ASC)
    INCLUDE([BF_SSN], [LN_SEQ], [LC_TYP_SCH_DIS], [LA_RPS_ISL]);


GO
CREATE NONCLUSTERED INDEX [IX_BFSSN_LNSEQ_LNRPSSEQ_CurrentGradation]
    ON [calc].[RepaymentSchedules]([BF_SSN] ASC, [LN_SEQ] ASC, [LN_RPS_SEQ] ASC, [CurrentGradation] ASC)
    INCLUDE([LC_TYP_SCH_DIS], [LN_RPS_TRM]);

