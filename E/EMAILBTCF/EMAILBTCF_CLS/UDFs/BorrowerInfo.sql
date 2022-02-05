CREATE TYPE [emailbtcf].[BorrowerInfo] AS TABLE
(
    [Recipient] VARCHAR(254) NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [FirstName] VARCHAR(100) NOT NULL, 
    [LastName] VARCHAR(100) NOT NULL
)
