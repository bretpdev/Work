CREATE TABLE [pmtlnhist].[LoanData]
(
	[LoanDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] VARCHAR(10) NOT NULL, 
    [ClaimId] VARCHAR(4) NOT NULL, 
    [UniqueId] VARCHAR(19) NOT NULL, 
    [LoanDate] DATE NOT NULL, 
    [PaymentType] VARCHAR(10) NOT NULL , 
    [PaymentAmount] FLOAT NOT NULL DEFAULT 0, 
    [Principal] FLOAT NOT NULL DEFAULT 0, 
    [Interest] FLOAT NOT NULL DEFAULT 0, 
    [LegalCosts] FLOAT NOT NULL DEFAULT 0, 
    [OtherCosts] FLOAT NOT NULL DEFAULT 0, 
    [CollectionCosts] FLOAT NOT NULL DEFAULT 0, 
    [Amount] FLOAT NOT NULL DEFAULT 0, 
    [Type] VARCHAR(10) NOT NULL , 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
