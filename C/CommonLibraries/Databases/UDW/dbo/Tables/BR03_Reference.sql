CREATE TABLE [dbo].[BR03_Reference] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [DF_PRS_ID_RFR] VARCHAR (9)  NOT NULL,
    [BC_STA_BR03]   VARCHAR (1)  CONSTRAINT [DF_BR03_Reference_BC_STA_BR03] DEFAULT ('Z') NOT NULL,
    [BI_ATH_3_PTY]  VARCHAR (1)  CONSTRAINT [DF_BR03_Reference_BI_ATH_3_PTY] DEFAULT ('Z') NOT NULL,
    [BC_RFR_REL_BR] VARCHAR (2)  CONSTRAINT [DF_BR03_Reference_BC_RFR_REL_BR] DEFAULT ('Z') NOT NULL,
    [BM_RFR_1]      VARCHAR (12) CONSTRAINT [DF_BR03_Reference_BM_RFR_1] DEFAULT ('Z') NOT NULL,
    [BM_RFR_LST]    VARCHAR (35) CONSTRAINT [DF_BR03_Reference_BM_RFR_LST] DEFAULT ('Z') NOT NULL,
    [LST_CNC]       VARCHAR (10) CONSTRAINT [DF_BR03_Reference_LST_CNC] DEFAULT ('Z') NOT NULL,
    [LST_ATT]       VARCHAR (10) CONSTRAINT [DF_BR03_Reference_LST_ATT] DEFAULT ('Z') NOT NULL,
    [RFR_REL_BR]    VARCHAR (13) CONSTRAINT [DF_BR03_Reference_RFR_REL_BR] DEFAULT ('Z') NOT NULL,
    CONSTRAINT [PK_BR03_Reference] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [DF_PRS_ID_RFR] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last attempted contact date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'LST_ATT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last contact date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'LST_CNC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference last name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'BM_RFR_LST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference first name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'BM_RFR_1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference relationship to borrower code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'BC_RFR_REL_BR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'BC_STA_BR03';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Person ID of the reference', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'DF_PRS_ID_RFR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BR03_Reference', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';

