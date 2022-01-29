CREATE TABLE [rtnemlinvf].[EmailAddresses]
(
	[EmailAddressesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailAddress] VARCHAR(254) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
