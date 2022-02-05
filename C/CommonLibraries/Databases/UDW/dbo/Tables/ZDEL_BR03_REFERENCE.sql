CREATE TABLE [dbo].[ZDEL_BR03_REFERENCE] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [DF_PRS_ID_RFR] VARCHAR (9)  NOT NULL,
    CONSTRAINT [PK_REFERENCE_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DF_PRS_ID_RFR] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Person ID of the reference', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BR03_REFERENCE', @level2type = N'COLUMN', @level2name = N'DF_PRS_ID_RFR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BR03_REFERENCE', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';

