CREATE TABLE [dbo].[LN55_Benefit] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NOT NULL,
    [LN_SEQ]             INT            NOT NULL,
    [LD_BBS_DSQ]         VARCHAR (10)   NULL,
    [LC_BBS_ELG]         VARCHAR (1)    NULL,
    [PR_EFT_RIR]         NUMERIC (5, 3) NULL,
    [PM_BBS_PGM]         VARCHAR (3)    NULL,
    [LN_BBS_STS_PCV_PAY] INT            NULL,
    [PN_BBT_DLQ_MOT]     INT            NULL,
    [RIR_VAL]            NUMERIC (8, 3) NULL,
    [RIR_CT]             AS             ((CONVERT([varchar](4),[LN_BBS_STS_PCV_PAY],0)+'/')+CONVERT([varchar](4),[PN_BBT_DLQ_MOT],0)),
    [RIR_INT]            AS             ((CONVERT([varchar](8),[RIR_VAL],0)+' ')+[PM_BBS_PGM]),
    [RIR_TYP]            NCHAR (10)     NULL,
    CONSTRAINT [PK_Borrower_Benefit] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date loan disqualified from the borrower benefit program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'LD_BBS_DSQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower benefit system program eligibility code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'LC_BBS_ELG';

