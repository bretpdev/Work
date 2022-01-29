CREATE TABLE [dbo].[NTISReturnContactInfo]
(
	[NTISReturnContactInfoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReturnName] VARCHAR(100) NOT NULL, 
    [AddressLine1] VARCHAR(75) NOT NULL, 
    [AddressLine2] VARCHAR(75) NULL, 
    [AddressLine3] VARCHAR(75) NULL, 
    [City] VARCHAR(75) NOT NULL, 
    [State] VARCHAR(5) NOT NULL, 
    [ReturnZip] VARCHAR(5) NOT NULL, 
    [ReturnZipAddOn] VARCHAR(4) NULL
)
