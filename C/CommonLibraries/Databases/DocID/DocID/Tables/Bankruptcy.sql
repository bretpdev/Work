CREATE TABLE [dbo].[Bankruptcy] (
    [Acc]    VARCHAR (10) NOT NULL,
    [CaseID] VARCHAR (15) NOT NULL,
    [FName]  VARCHAR (50) NOT NULL,
    [LName]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Bankruptcy] PRIMARY KEY CLUSTERED ([Acc] ASC, [CaseID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bankruptcy', @level2type = N'COLUMN', @level2name = N'Acc';

