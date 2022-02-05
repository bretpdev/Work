CREATE TABLE [billing].[BillingHeaders]
(
	[BillingHeaderId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BillingHeader] VARCHAR(1000) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [InactivatedAt] DATETIME NULL
)