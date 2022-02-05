CREATE TABLE [dbo].[BR02_Employer] (
    [DF_SPE_ACC_ID]    VARCHAR (10) NOT NULL,
    [IM_IST_FUL]       VARCHAR (40) CONSTRAINT [DF_Employer_IM_IST_FUL] DEFAULT (' ') NULL,
    [IX_GEN_STR_ADR_1] VARCHAR (40) CONSTRAINT [DF_Employer_IX_GEN_STR_ADR_1] DEFAULT (' ') NULL,
    [IX_GEN_STR_ADR_2] VARCHAR (40) CONSTRAINT [DF_Employer_IX_GEN_STR_ADR_2] DEFAULT (' ') NULL,
    [IM_GEN_CT]        VARCHAR (30) CONSTRAINT [DF_Employer_IM_GEN_CT] DEFAULT (' ') NULL,
    [IC_GEN_ST]        VARCHAR (2)  CONSTRAINT [DF_Employer_IC_GEN_ST] DEFAULT (' ') NULL,
    [IF_GEN_ZIP]       VARCHAR (14) CONSTRAINT [DF_Employer_IF_GEN_ZIP] DEFAULT (' ') NULL,
    [IN_GEN_PHN]       VARCHAR (17) CONSTRAINT [DF_Employer_IN_GEN_PHN] DEFAULT (' ') NULL,
    CONSTRAINT [PK_Employer] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer''s name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IM_IST_FUL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer street address 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IX_GEN_STR_ADR_1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer street address 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IX_GEN_STR_ADR_2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer city', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IM_GEN_CT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer state code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IC_GEN_ST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer zip code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IF_GEN_ZIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Employer phone number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR02_Employer', @level2type = N'COLUMN', @level2name = N'IN_GEN_PHN';

