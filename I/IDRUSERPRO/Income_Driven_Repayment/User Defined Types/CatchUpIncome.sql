CREATE TYPE [dbo].[CatchUpIncome] AS TABLE (
    [RepayeEndDate] DATETIME        NULL,
    [CreateDate]    DATETIME        NULL,
    [Year]          INT             NULL,
    [AGI]           DECIMAL (18, 2) NULL,
    [State]         CHAR (2)        NULL,
    [FamilySize]    INT             NULL,
    [Source]        VARCHAR (3)     NULL,
    [ExternalDebt]  DECIMAL (18, 2) NULL);

