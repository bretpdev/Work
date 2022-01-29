CREATE TABLE [dbo].[AY10_WaiveFee] (
    [DF_SPE_ACC_ID] VARCHAR (10)   NOT NULL,
    [FEE_WAV_DOL]   DECIMAL (8, 2) CONSTRAINT [DF_Table_1_fee_wav_dol] DEFAULT ((0)) NULL,
    [FEE_WAV_CT]    INT            CONSTRAINT [DF_Table_1_fee_wav_ct] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Waived_Fees] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_WaiveFee', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dollar amount of waived fees for borrower', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_WaiveFee', @level2type = N'COLUMN', @level2name = N'FEE_WAV_DOL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Count of fees waived for borrower', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_WaiveFee', @level2type = N'COLUMN', @level2name = N'FEE_WAV_CT';

