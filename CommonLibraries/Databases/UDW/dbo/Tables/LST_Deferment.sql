CREATE TABLE [dbo].[LST_Deferment] (
    [DEFERMENT_CODE] VARCHAR (2)  NOT NULL,
    [MAX_MONTHS]     INT          NULL,
    [DESCRIPTION]    VARCHAR (50) NULL,
    CONSTRAINT [PK_LST_Deferment] PRIMARY KEY CLUSTERED ([DEFERMENT_CODE] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LST_Deferment', @level2type = N'COLUMN', @level2name = N'DESCRIPTION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Months allowed for type of deferment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LST_Deferment', @level2type = N'COLUMN', @level2name = N'MAX_MONTHS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LST_Deferment', @level2type = N'COLUMN', @level2name = N'DEFERMENT_CODE';

