CREATE TABLE [dbo].[ZDEL_PD32_EMAIL] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [DC_ADR_EML]    VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_EMAIL_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DC_ADR_EML] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_PD32_EMAIL', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'E-mail address type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_PD32_EMAIL', @level2type = N'COLUMN', @level2name = N'DC_ADR_EML';

