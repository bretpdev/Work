CREATE TABLE [dbo].[PD42_Phone] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [DC_PHN]         VARCHAR (1)  NOT NULL,
    [DD_PHN_VER]     VARCHAR (10) NULL,
    [DI_PHN_VLD]     VARCHAR (1)  NULL,
    [DN_DOM_PHN_ARA] VARCHAR (3)  NULL,
    [DN_DOM_PHN_XCH] VARCHAR (3)  NULL,
    [DN_DOM_PHN_LCL] VARCHAR (4)  NULL,
    [DN_PHN_XTN]     VARCHAR (5)  NULL,
    [DN_FGN_PHN_CNY] VARCHAR (3)  NULL,
    [DN_FGN_PHN_CT]  VARCHAR (5)  NULL,
    [DN_FGN_PHN_LCL] VARCHAR (11) NULL,
    [CONSENT_IND]    VARCHAR (1)  NULL,
    [LINE_TYP]       VARCHAR (1)  NULL,
    [IVR_PHONE]      VARCHAR (10) NULL,
    CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DC_PHN] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DC_PHN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date phone number vierified(mm/dd/yyyy)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DD_PHN_VER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone valid indicator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DI_PHN_VLD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone area code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_DOM_PHN_ARA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_DOM_PHN_XCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_DOM_PHN_LCL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone extension', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_PHN_XTN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign phone country number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_FGN_PHN_CNY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign phone city number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_FGN_PHN_CT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign phone local number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'DN_FGN_PHN_LCL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Consent to contact phone indicator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'CONSENT_IND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type of phone number(Mobile, Landline)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD42_Phone', @level2type = N'COLUMN', @level2name = N'LINE_TYP';

