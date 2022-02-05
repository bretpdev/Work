CREATE TABLE [dbo].[IvrCheckByPhone] (
    [RecNum]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [AccountNumber]  VARCHAR (10) NOT NULL,
    [BankAccountNum] VARCHAR (50) NOT NULL,
    [AccountType]    VARCHAR (10) NOT NULL,
    [RoutingNum]     VARCHAR (50) NOT NULL,
    [Amount]         MONEY        NOT NULL,
    [ProcessedDate]  DATETIME     NULL,
    [AuthDate]       DATETIME     NULL
);

