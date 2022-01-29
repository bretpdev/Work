﻿CREATE TABLE [dbo].[ZDEL_BR02_EMPLOYER] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_EMPLOYER_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BR02_EMPLOYER', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';

