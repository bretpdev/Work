CREATE TABLE [rcdialer].[DoNotContact]
(
	[DoNotContactId] INT NOT NULL PRIMARY KEY IDENTITY,
    [BorrowerId] INT NOT NULL,
    [PhoneNumber] VARCHAR(20) NOT NULL,
    [Reason] VARCHAR(200) NOT NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    [DeletedAt] DATETIME NULL
)
