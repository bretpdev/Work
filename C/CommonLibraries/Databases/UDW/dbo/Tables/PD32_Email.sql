CREATE TABLE [dbo].[PD32_Email] (
    [DF_SPE_ACC_ID]  VARCHAR (10)  NOT NULL,
    [DC_ADR_EML]     VARCHAR (1)   NOT NULL,
    [DX_ADR_EML]     VARCHAR (254) NULL,
    [DD_VER_ADR_EML] VARCHAR (10)  NULL,
    [DI_VLD_ADR_EML] VARCHAR (1)   NULL,
    CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DC_ADR_EML] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Validity indicator for this e-mail address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD32_Email', @level2type = N'COLUMN', @level2name = N'DI_VLD_ADR_EML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date e-mail address verified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD32_Email', @level2type = N'COLUMN', @level2name = N'DD_VER_ADR_EML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'E-mail address text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD32_Email', @level2type = N'COLUMN', @level2name = N'DX_ADR_EML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'E-mail address type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD32_Email', @level2type = N'COLUMN', @level2name = N'DC_ADR_EML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD32_Email', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';

