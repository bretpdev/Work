CREATE TABLE [cpp].[PaymentSources]
(
	[PaymentSourcesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PaymentSource] VARCHAR(20) NOT NULL, 
    [InstitutionId] CHAR(6) NOT NULL, 
    [FileName] VARCHAR(25) NOT NULL,
    [FileTypeId] INT NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1, 
    [CreatedAt] DATE NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    CONSTRAINT [FK_PaymentSources_PaymentSourceType] FOREIGN KEY ([FileTypeId]) REFERENCES [cpp].[FileTypes]([FileTypeId])
)