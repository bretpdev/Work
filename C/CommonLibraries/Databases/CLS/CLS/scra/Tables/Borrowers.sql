CREATE TABLE [scra].[Borrowers]
(
	[BorrowerId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BorrowerAccountNumber] CHAR(10) NULL, 
    [EndorserAccountNumber] CHAR(10) NULL
)
