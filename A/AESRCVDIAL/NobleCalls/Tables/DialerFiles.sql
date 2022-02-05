CREATE TABLE [aesrcvdial].[DialerFiles]
(
	[DialerFilesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileName] VARCHAR(50) NOT NULL,
    [OutputFileName] VARCHAR(50) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1, 
    [IsRequired] BIT NOT NULL DEFAULT 1,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
