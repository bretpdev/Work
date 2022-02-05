CREATE TABLE [dbo].[BR30_Autopay] (
    [DF_SPE_ACC_ID]  VARCHAR (10)   NOT NULL,
    [BN_EFT_SEQ]     INT            CONSTRAINT [DF_Autopay_BN_EFT_SEQ] DEFAULT ((0)) NULL,
    [BF_EFT_ABA]     VARCHAR (9)    CONSTRAINT [DF_Autopay_BF_EFT_ABA] DEFAULT (' ') NULL,
    [BF_EFT_ACC]     VARCHAR (17)   CONSTRAINT [DF_Autopay_BF_EFT_ACC] DEFAULT (' ') NULL,
    [BC_EFT_STA]     VARCHAR (1)    CONSTRAINT [DF_Autopay_BC_EFT_STA] DEFAULT (' ') NULL,
    [BD_EFT_STA]     VARCHAR (10)   CONSTRAINT [DF_Autopay_BD_EFT_STA] DEFAULT (' ') NULL,
    [BA_EFT_ADD_WDR] NUMERIC (8, 2) CONSTRAINT [DF_Autopay_BA_EFT_ADD_WDR] DEFAULT ((0)) NULL,
    [BN_EFT_NSF_CTR] SMALLINT       CONSTRAINT [DF_Autopay_BN_EFT_NSF_CTR] DEFAULT ((0)) NULL,
    [BC_EFT_DNL_REA] VARCHAR (1)    CONSTRAINT [DF_Autopay_BC_EFT_DNL_REA] DEFAULT (' ') NULL,
    CONSTRAINT [PK_Autopay] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EFT information sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BN_EFT_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ABA number for EFT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BF_EFT_ABA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EFT account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BF_EFT_ACC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EFT status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BC_EFT_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EFT status date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BD_EFT_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Additional withdrawal amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BA_EFT_ADD_WDR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Non-sufficient funds counter', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BN_EFT_NSF_CTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EFT denial reason code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR30_Autopay', @level2type = N'COLUMN', @level2name = N'BC_EFT_DNL_REA';

