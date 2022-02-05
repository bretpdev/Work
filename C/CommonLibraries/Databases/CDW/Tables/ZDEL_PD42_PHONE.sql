CREATE TABLE [dbo].[ZDEL_PD42_PHONE] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [DC_PHN]        VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_PHONE_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DC_PHN] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_PD42_PHONE', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_PD42_PHONE', @level2type = N'COLUMN', @level2name = N'DC_PHN';

